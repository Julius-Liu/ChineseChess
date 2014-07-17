using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Sockets;

namespace ChineseChess
{
    /// <summary>
    /// ����Ĺ���
    /// </summary>
	public class Rules
	{
        private static byte[] buffer = new byte[8192];

		public static Point Locate(View view, Point p)
		{
			int x=0, y=0;

			for (int j = 0; j < 8; j++)
			{
			    if (p.X <= view.col_range[j])
			    {
			        x = view.col_pixel[j];
			        break;
			    }
				x = view.col_pixel[8];
			}

			for (int i = 0; i < 9; i++)
			{
				if (p.Y <= view.row_range[i])
				{
					y = view.row_pixel[i];
					break;
				}
				y = view.row_pixel[9];
			}

			Point result = new Point(x,y);
			return result;
		}

		public static void DoMove(View view, PictureBox pictureBox, int x1, int y1, int x2, int y2)
		{
			view.chessBoard[y1, x1] = null;
			pictureBox.Location = new Point(view.col_pixel[x2], view.row_pixel[y2]);
                
            if (view.chessBoard[y2, x2] != null)  // ���ӵ����
            {
                view.chessBoard[y2, x2].Enabled = false;
                view.chessBoard[y2, x2].Visible = false;
            }
            view.chessBoard[y2, x2] = pictureBox;			
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkStream"></param>
        /// <param name="msgType"> pawn_move �����ƶ� who_first ������� chat_msg ������Ϣ</param>
        /// <param name="id"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static void SendMessage(NetworkStream networkStream, MessageEntity messageEntity)
        {
            try
            {
                buffer = Encoding.Unicode.GetBytes(messageEntity.GetMsgType() + "+" + messageEntity.GetMsgContent());
                
                lock (networkStream)
                {
                    networkStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// �Խ��յ��ĶԷ����ƶ����н��룬ת�����Լ������еĶԷ����ƶ�
        /// </summary>
        /// <param name="text"> ��Ϣ</param>
        /// <returns> ����������</returns>
        //public static string[] DecodeMessage(string text)
        //{
        //    //string[] result = new int[5];
        //    string[] array = text.Split('_');
        //    result[0] = int.Parse(array[0]);
        //    result[1] = 8 - int.Parse(array[1]);
        //    result[2] = 9 - int.Parse(array[2]);
        //    result[3] = 8 - int.Parse(array[3]);
        //    result[4] = 9 - int.Parse(array[4]);

        //    return result;
        //}

        //private static int GetType(PictureBox pictureBox)
        //{
        //    if (pictureBox.Name.Contains("jiang"))
        //        return 0;
        //    else if (pictureBox.Name.Contains("shi"))
        //        return 1;
        //    else if (pictureBox.Name.Contains("xiang"))
        //        return 2;
        //    else if (pictureBox.Name.Contains("ma"))
        //        return 3;
        //    else if (pictureBox.Name.Contains("ju"))
        //        return 4;
        //    else if (pictureBox.Name.Contains("pao"))
        //        return 5;
        //    else
        //        return 6;
        //}

        public static bool CheckJiang(View view, int Rjiang_x, int Rjiang_y, int Bjiang_x, int Bjiang_y)
        {
            if (Rjiang_x == Bjiang_x)
            {
                for (int i = Bjiang_y + 1; i < Rjiang_y; i++)
                {
                    if (view.chessBoard[i, Rjiang_x] != null)  // ����м������ӣ�˵��û�н��Խ�
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

		public static bool CanMove(View view, string pawnType, int x1, int y1, int x2, int y2)
		{
            if (view.chessBoard[y1, x1].Name.Contains("R")
                && view.chessBoard[y2, x2] != null
                && view.chessBoard[y2, x2].Name.Contains("R"))
            {               
                // ����Լ����ӳ��Լ�����
                return false;               
            }            
            else
            {
			    switch (pawnType)
			    { 
				    case "jiang":  // ����˧���ƶ�
                        if (x1 == x2 || y1 == y2)  // ��ֱ�ƶ���ˮƽ�ƶ�
                        {
                            if (x1 == x2)  // ��ֱ�ƶ�
                            {
                                if (Math.Abs(y2 - y1) != 1)  // �����ƶ�һ�񣬴���
                                {
                                    return false;
                                }
                                if (y2 < 7)  // �������Ӹ񣬴���
                                {
                                    return false;
                                }
                                return true;  // ����������������������ƶ�
                            }
                            else  // ˮƽ�ƶ�
                            {
                                if (Math.Abs(x2 - x1) != 1)  // �����ƶ�һ�񣬴���
                                {
                                    return false;
                                }
                                if (x2 < 3 || x2 > 5)  // �������Ӹ񣬴���
                                {
                                    return false;
                                }
                                return true;  // ����������������������ƶ�
                            }
                        }
                        else  // ���Ǵ�ֱ�ƶ���ˮƽ�ƶ��������ƶ�
                        {
                            return false;
                        }

				    case "shi":  // ʿ���ƶ�
                        if (x2 > 2 && x2 < 6 && y2 > 6)  // ��������ַ�Χ��
                        {
                            if (Math.Abs(x2 - x1) == 1 && Math.Abs(y2 - y1) == 1)
                            {
                                return true; // ���б���ƶ�һ����ȷ
                            }
                            else
                            {
                                return false; // �������б���ƶ�һ�񣬴���
                            }
                        }
                        else   // ��������ַ�Χ�⣬����
                        {
                            return false;
                        }

				    case "xiang":  // ����ƶ�
                        if ((x2 == 0 || x2 == 2 || x2 == 4 || x2 == 6 || x2 == 8) && 
                            (y2 == 5 || y2 == 7 || y2 == 9))  // �������ó��ֵķ�Χ��
                        {
                            if (Math.Abs(x2 - x1) == 2 && Math.Abs(y2 - y1) == 2) // ���б���ƶ�����
                            {
                                if (view.chessBoard[(y1 + y2) / 2, (x1 + x2) / 2] == null)
                                {
                                    return true;  // ���û���ϰ�����ȷ
                                }
                                else
                                {
                                    return false;  // ������ϰ�������
                                }
                            }
                            else   // �������б���ƶ����񣬴���
                            {
                                return false;
                            }
                        }
                        else   // ������ڷ�Χ�ڣ�����
                        {
                            return false;
                        }

				    case "ma":  // �����ƶ�
                        if (Math.Abs(x1 - x2) == 1 && Math.Abs(y1 - y2) == 2) // ��������������ƶ�
                        {
                            if (view.chessBoard[(y1 + y2) / 2, x1] == null) // ���û���ϰ�����ȷ
                            {
                                return true;
                            }
                            else  // ������ϰ�������
                            {
                                return false;
                            }
                        }
                        else if (Math.Abs(y1 - y2) == 1 && Math.Abs(x1 - x2) == 2) // ����Ǻ�������ƶ�
                        {
                            if (view.chessBoard[y1, (x1 + x2) / 2] == null) // ���û���ϰ�����ȷ
                            {
                                return true;
                            }
                            else  // ������ϰ�������
                            {
                                return false;
                            }
                        }
                        else  // ������������ƶ�������
                        {
                            return false;
                        }

				    case "ju":  // �����ƶ�
                        if (x1 == x2 || y1 == y2)  // �����ֱ���ƶ�
                        {
                            if (x1 == x2)  // ����Ǵ�ֱ�ƶ�
                            {
                                if (y2 > y1)  // ������������ƶ�
                                {
                                    for (int i = y1 + 1; i < y2; i++)
                                    {
                                        if (view.chessBoard[i, x1] != null)  // ����м������ӣ����˳�
                                        {
                                            return false;
                                        }
                                    }
                                }
                                else  // ������������ƶ�
                                {
                                    for (int i = y2 + 1; i < y1; i++)
                                    {
                                        if (view.chessBoard[i, x1] != null)  // ����м������ӣ����˳�
                                        {
                                            return false;
                                        }
                                    }
                                }
                                return true;  // �м�û�����ӣ������ƶ�
                            }
                            else  // ���ˮƽ�ƶ�
                            {
                                if (x2 > x1)  // ������������ƶ�
                                {
                                    for (int i = x1 + 1; i < x2; i++)
                                    {
                                        if (view.chessBoard[y1, i] != null)  // ����м������ӣ����˳�
                                        {
                                            return false;
                                        }
                                    }
                                }
                                else  // ������������ƶ�
                                {
                                    for (int i = x2 + 1; i < x1; i++)
                                    {
                                        if (view.chessBoard[y1, i] != null)  // ����м������ӣ����˳�
                                        {
                                            return false;
                                        }
                                    }
                                }
                                return true;  // �м�û�����ӣ������ƶ�
                            }
                        }
                        else  // �������ֱ���ƶ����ͳ���
                        {
                            return false;
                        }

				    case "pao":  // �ڵ��ƶ�
                        if (x1 == x2 || y1 == y2) // �����ֱ���ƶ�
                        {
                            if (x1 == x2)  // ����Ǵ�ֱ�ƶ�
                            {                                
                                if (view.chessBoard[y2, x2] == null)   // ���ֻ���ƶ�
                                {
                                    if (y2 > y1)  // ������������ƶ�
                                    {
                                        for (int i = y1 + 1; i < y2; i++)
                                        {
                                            if (view.chessBoard[i, x1] != null)  // ����м������ӣ����˳�
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    else  // ������������ƶ�
                                    {
                                        for (int i = y2 + 1; i < y1; i++)
                                        {
                                            if (view.chessBoard[i, x1] != null)  // ����м������ӣ����˳�
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    return true;  // �м�û�����ӣ������ƶ�
                                }
                                else // ����ǳԵ��ڷ�����
                                {
                                    if (y2 > y1)  // ������������ƶ�
                                    {
                                        int count = 0;
                                        for (int i = y1 + 1; i < y2; i++)
                                        {
                                            if (view.chessBoard[i, x1] != null)
                                            {
                                                count++;
                                            }
                                        }
                                        if (count == 1)  // ����һ�����ӣ���ȷ
                                        {
                                            return true;
                                        }
                                        else  // û���������ӣ�������������һ�����ӣ�����
                                        {
                                            return false;
                                        }
                                    }
                                    else  // ������������ƶ�
                                    {
                                        int count = 0;
                                        for (int i = y2 + 1; i < y1; i++)
                                        {
                                            if (view.chessBoard[i, x1] != null)
                                            {
                                                count++;
                                            }
                                        }
                                        if (count == 1)  // ����һ�����ӣ���ȷ
                                        {
                                            return true;
                                        }
                                        else  // û���������ӣ�������������һ�����ӣ�����
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else  // ���ˮƽ�ƶ�
                            {                                
                                if (view.chessBoard[y2, x2] == null)  // ���ֻ���ƶ�
                                {
                                    if (x2 > x1)  // ������������ƶ�
                                    {
                                        for (int i = x1 + 1; i < x2; i++)
                                        {
                                            if (view.chessBoard[y1, i] != null)  // ����м������ӣ����˳�
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    else  // ������������ƶ�
                                    {
                                        for (int i = x2 + 1; i < x1; i++)
                                        {
                                            if (view.chessBoard[y1, i] != null)  // ����м������ӣ����˳�
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    return true;  // �м�û�����ӣ������ƶ�
                                }
                                else // ����ǳԵ��ڷ�����
                                {
                                    if (x2 > x1)  // ������������ƶ�
                                    {
                                        int count = 0;
                                        for (int i = x1 + 1; i < x2; i++)
                                        {
                                            if (view.chessBoard[y1, i] != null)
                                            {
                                                count++;
                                            }
                                        }
                                        if (count == 1)  // ����һ�����ӣ���ȷ
                                        {
                                            return true;
                                        }
                                        else  // û���������ӣ�������������һ�����ӣ�����
                                        {
                                            return false;
                                        }
                                    }
                                    else  // ������������ƶ�
                                    {
                                        int count = 0;
                                        for (int i = x2 + 1; i < x1; i++)
                                        {
                                            if (view.chessBoard[y1, i] != null)
                                            {
                                                count++;
                                            }
                                        }
                                        if (count == 1)  // ����һ�����ӣ���ȷ
                                        {
                                            return true;
                                        }
                                        else  // û���������ӣ�������������һ�����ӣ�����
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        else  // �������ֱ���ƶ������˳�
                        {
                            return false;
                        }

				    case "bing":  // �����ƶ�
                        if (y1 > 4)     // ������Լ��ĺӽ���
                        {
                            if (x1 == x2 && (y1 - y2) == 1) // �����ǰ�ƶ�һ������ȷ
                            {
                                return true;
                            }
                            else  // ���������ô��������
                            {
                                return false;
                            }
                        }
                        else   // ����ںڷ��ĺӽ���
                        {
                            if (y2 == y1 && Math.Abs(x1 - x2) == 1) // ��������ƶ�һ������ȷ
                            {
                                return true;
                            }
                            else if ((y1 - y2) == 1 && x1 == x2) // �����ǰ�ƶ�һ������ȷ
                            {
                                return true;
                            }
                            else   // ������������������������
                            {
                                return false;
                            }
                        }
                    default:   // �������������
                        return false;
			    }	
            }
		}

	}
}