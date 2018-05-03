using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Messaging;


namespace InterfaceChess
{
    static public class QueueMsg
    {
        static private MessageQueue m_qSetupBoard = null;
        static private MessageQueue m_qPreLoadBoard = null;
        static private MessageQueue m_qMoves = null;
        static private MessageQueue m_mq_send_to_OW = null;
        static private MessageQueue m_qColor = null;
   
        public static void InitQueue()
        {
            if (MessageQueue.Exists(@".\Private$\MyQueue"))
                m_qMoves = new System.Messaging.MessageQueue(@".\Private$\MyQueue");
            else
                m_qMoves = MessageQueue.Create(@".\Private$\MyQueue");

            if (MessageQueue.Exists(@".\Private$\MyQueueOW"))
                m_mq_send_to_OW = new System.Messaging.MessageQueue(@".\Private$\MyQueueOW");
            else
                m_mq_send_to_OW = MessageQueue.Create(@".\Private$\MyQueueOW");    
        }

        public static void Init_Color()
        {
            if (MessageQueue.Exists(@".\Private$\MyQueueColor"))
                m_qColor = new System.Messaging.MessageQueue(@".\Private$\MyQueueColor");
            else
                m_qColor = MessageQueue.Create(@".\Private$\MyQueueColor");

            while (ReadMessage_Color() != K.Msg) ;
        }

        static public void Init_ConfigBoard()
        {
            if (MessageQueue.Exists(@".\Private$\MyQueueConfig"))
                m_qSetupBoard = new System.Messaging.MessageQueue(@".\Private$\MyQueueConfig");
            else
                m_qSetupBoard = MessageQueue.Create(@".\Private$\MyQueueConfig");

           // while (ReadMessage_SetupBoard() != "No Message") ;

        }

        static public void Init_PreLoadBoard()
        {
            if (MessageQueue.Exists(@".\Private$\PreLoad"))
                m_qPreLoadBoard = new System.Messaging.MessageQueue(@".\Private$\PreLoad");
            else
                m_qPreLoadBoard = MessageQueue.Create(@".\Private$\PreLoad");

           while (ReadMessage_PreLoadBoard() != K.Msg);

        }

        static public String ReadMessage_ConfigBoard()
        {
            String message = String.Empty;

            System.Messaging.Message mes;

            try
            {
                mes = m_qSetupBoard.Receive(new TimeSpan(0, 0, 3));
                mes.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                message = mes.Body.ToString();
            }
            catch
            {
                message = K.Msg;
            }

            return (message);
        }

        public static String ReadMessage_Color()
        {
            String message = String.Empty;

            System.Messaging.Message mes;

            try
            {
                mes = m_qColor.Receive(new TimeSpan(0, 0, 3));
                mes.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                message = mes.Body.ToString();
            }
            catch
            {
                message = K.Msg;
            }

            return (message);
        }
        public static String ReadQueue()
        {
            String message = String.Empty;

            System.Messaging.Message mes;

            try
            {
                mes = m_qMoves.Receive(new TimeSpan(0, 0, 3));
                mes.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                message = mes.Body.ToString();
            }
            catch
            {
                message = K.Msg;
            }

            return (message);
        }

        static public String ReadMessage_PreLoadBoard()
        {
            String message = String.Empty;

            System.Messaging.Message mes;

            try
            {
                mes = m_qPreLoadBoard.Receive(new TimeSpan(0, 0, 3));
                mes.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                message = mes.Body.ToString();
            }
            catch
            {
                message = K.Msg;
            }

            return (message);
        }

        public static void SendMessage_To_OpenWindow(String message)
        {
            System.Messaging.Message mm = new System.Messaging.Message();
            mm.Body = message;
            mm.Label = "Msg";
            m_mq_send_to_OW.Send(mm);
        }

        public static Boolean isClickDown(String msg)
        {
            Boolean bClickDown = false;

            if (msg.Contains("Down"))
                bClickDown = true;

            return (bClickDown);
        }

        public static Boolean isClickUp(String msg)
        {
            Boolean bClickUp = false;

            if (msg.Contains("Up"))
                bClickUp = true;

            return (bClickUp);
        }

        public static void GetCoordonneeBoard(String message, out int x, out int y)
        {
            byte i = 1;
            string data = string.Empty;

            x = y = -1;

            if (message.Contains("="))
            {
                message = message.Substring(message.IndexOf("=") + 1);

                string[] words = message.Split(';');

                foreach (string word in words)
                {
                    data = word.Substring(word.IndexOf('=') + 1);

                    try
                    {
                        if (i == 1) x = Convert.ToInt32(data);
                        else if (i == 2) y = Convert.ToInt32(data);
                    }
                    catch
                    {
                    }

                    i++;
                }

                if (x > 1600)
                {
                    i = 0;
                }
            }

        }

    }


}