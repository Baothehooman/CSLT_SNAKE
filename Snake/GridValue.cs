namespace Snake
{
    public enum GridValue //Enum được sử dụng để định nghĩa các kiểu dữ liệu liệt kê
    {
        Empty, // ô rỗng trong lưới ô
        Snake, //chứa các phần đầu thân đuôi của con rắn
        Food, //chứa mồi
        Outside //không được chứa trong giá trị của ô grid, nhưng được sd khi con rắn muốn vượt ra ngoài lưới ô
    }
}
