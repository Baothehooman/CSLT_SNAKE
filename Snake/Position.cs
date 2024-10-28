using System;
using System.Collections.Generic;

namespace Snake
{
    public class Position
    {
        public int Row { get; } //Định nghĩa hai thuộc tính chỉ đọc, Row và Col, cho hàng và cột của một vị trí
        public int Col { get; }

        public Position(int row, int col) //hàm khởi tạo một đối tượng Position với các giá trị hàng và cột cụ thể
        {
            Row = row;
            Col = col;
        }

        public Position Translate(Direction dir) //Phương thức dịch chuyển 1 bước từ vị trí hiện tại bởi một đối tượng Direction cụ thể, trả về một Position mới.
        {
            return new Position(Row + dir.RowOffset, Col + dir.ColOffset); // trả về một vị trí mới: cột = cột + gtri di chuyển của cột, hàng = hàng + gtri di chuyển của hàng
        }

        public override bool Equals(object obj) //kiểm tra xem Position hiện tại có bằng với một đối tượng khác, dựa trên giá trị hàng và cột
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}
