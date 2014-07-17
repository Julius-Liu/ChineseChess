using System;
using System.Collections.Generic;
using System.Text;

namespace ChineseChess
{
    public class MessageEntity
    {
        private string msgType;
        private string pawnName;
        private int start_x, start_y, end_x, end_y;

        public MessageEntity(string msgType, string pawnName, 
            int start_x, int start_y, int end_x, int end_y)
        {
            this.msgType = msgType;
            this.pawnName = pawnName;
            this.start_x = start_x;
            this.start_y = start_y;
            this.end_x = end_x;
            this.end_y = end_y;
        }

        public string GetMsgType()
        {
            return this.msgType;
        }

        public string GetMsgContent()
        {
            string result = "";
            switch (this.msgType)
            { 
                case "pawn_move":
                    result = this.pawnName + "_" + this.start_x.ToString() + "_" +
                        this.start_y.ToString() + "_" + this.end_x.ToString() + "_" +
                        this.end_y.ToString();
                    break;
                case "who_first":
                    break;
                case "chat_msg":
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
