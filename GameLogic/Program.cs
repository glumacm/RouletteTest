using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace GameLogic { 

    [Serializable]
    class GameLogicServer
    {
        private static int i = 0;
        private static Boolean mainThreadEnd = false;
        private static Thread[] servers = new Thread[2];

        public static void Main(string[] args)
        {

            //Ustvarimo nit in ji dolocimo callback, ki se izvede, ko bo nit nekaj pognala.;
            Thread[] servers = new Thread[1];//;
            servers[0] = new Thread(ServerThread);
            servers[0].Start();
            while (true)
            {

                // ce hocemo, da se ves cas caka na povezavo potem moramo imeti infinite loop
                if (mainThreadEnd)
                {
                    servers[0] = new Thread(ServerThread);
                    servers[0].Start();
                    mainThreadEnd = false;
                }


            }
            Console.WriteLine("\nServer has no existing threads.");
        }

        public static TestTransport[] getSerializedObject(NamedPipeServerStream npss)
        {
            StringBuilder sb = new StringBuilder();
            String messageArrivedd = String.Empty;
            byte[] message = new byte[500];
            do
            {
                npss.Read(message, 0, message.Length);// nastavimo delcek sporocila v seznam
                messageArrivedd = Encoding.UTF8.GetString(message);
                sb.Append(messageArrivedd);
                message = new byte[message.Length];
            } while (!npss.IsMessageComplete);
            TestTransport[] tttt = JsonConvert.DeserializeObject<TestTransport[]>(sb.ToString());
            return tttt;
        }

        public static GameRound getSerializedRound(NamedPipeServerStream npss)
        {
            StringBuilder sb = new StringBuilder();
            String messageArrivedd = String.Empty;
            byte[] message = new byte[500];
            do
            {
                npss.Read(message, 0, message.Length);// nastavimo delcek sporocila v seznam
                messageArrivedd = Encoding.UTF8.GetString(message);
                sb.Append(messageArrivedd);
                message = new byte[message.Length];
            } while (!npss.IsMessageComplete);
            GameRound tttt = JsonConvert.DeserializeObject<GameRound>(sb.ToString());
            return tttt;
        }

        private static void ServerThread(object data) {
            //IFormatter f = 
            Console.WriteLine("Waiting for client...");
            PipeSecurity ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("gameLogicServer", PipeDirection.InOut,1, PipeTransmissionMode.Message,PipeOptions.None,1024,1024,ps);
            pipeServer.WaitForConnection();//s tem ustvarimo vsako nadaljno logiko, da se ne izvede dokler ne pride do neke povezave iz "clienta"
            IFormatter f = new BinaryFormatter();


            DateTime currenttime = DateTime.Now;

            GameRound game = getSerializedRound(pipeServer);

            //Console.WriteLine("prebralo pa je");
            //Console.WriteLine(game.winningNumber);
            List<Bet> bets = game.bets;
            for(int i = 0; i< bets.Count; i++)
            {
                
                Bet bet = bets[i];
                int winnable = returnWinningAmount(bet.betType, bet.betAmount, game.winningNumber, game.winningColor, bet.singleNumber);
                if(winnable > 0)
                {
                    bet.winnable = true;
                }
                bet.winAmount = winnable;
                bets[i] = bet;
            }
            game.bets = bets;


            DateTime afterProcess = DateTime.Now;
            byte[] sendResult = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(game));
            pipeServer.Write(sendResult, 0, sendResult.Length);
            pipeServer.Dispose();

            mainThreadEnd = true;
            pipeServer.Close();
            pipeServer = null;

        }


        private static int returnWinningAmount(String betType, int betAmount, int winningNumber, String winningColor, int betOnNumber)
        {
            if( 
                (betType == "black" && winningColor == "black") ||
                (betType == "red" && winningColor == "red") || 
                (betType=="odd" && (winningNumber % 2 == 1) ) ||
                (betType == "even" && winningNumber %2 ==0 && winningNumber != 0) ||
                (betType == "1to18" && winningNumber <= 18 && winningNumber >=1) ||
                (betType == "19to35" && winningNumber <= 35 && winningNumber >= 19)
                )
            {
                return betAmount * 2;
            }else if(betType == "singleNumber" && winningNumber == betOnNumber)
            {
                return 35 * betAmount;
            }else if(
                (betType == "firstDozen" && winningNumber >= 1 && winningNumber <= 12) ||
                (betType == "secondDozen" && winningNumber >= 13 && winningNumber <= 21) ||
                (betType == "firstDozen" && winningNumber >= 14 && winningNumber <= 35))
            {
                return 3 * betAmount;
            }
            else if (betType == "2to1_1")
            {
                for(int i =1; i<36; i+=3) {
                    if(winningNumber == i)
                    {
                        return 3 * betAmount;
                    }
                }
                return 0;
            }
            else if (betType == "2to1_2")
            {
                for (int i = 2; i < 36; i += 3)
                {
                    if (winningNumber == i)
                    {
                        return 3 * betAmount;
                    }
                }
                return 0;
            }
            else if (betType == "2to1_3")
            {
                for (int i = 3; i < 36; i += 3)
                {
                    if (winningNumber == i)
                    {
                        return 3 * betAmount;
                    }
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }

    }


    public class GameRound
    {
        public int winningNumber { get; set; }
        public String winningColor { get; set; }
        public List<Bet> bets { get; set; }
    }

    public class Bet
    {
        public int betAmount { get; set; }
        public String betType { get; set; } // firstDozen,secondDozen,thirdDozen, 1to18, 19to35, even,odd,singleNumber, black, red, 2to1_1, 2to1_2,2to1_3
        public int singleNumber { get; set; }

        public int winAmount { get; set; }

        public bool winnable { get; set; }
    }

    class TestTransport
    {
        public String firstname { get; set; }
        public String lastname { get; set; }

    }
}