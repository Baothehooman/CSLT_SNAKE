using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake
{
    // Lớp Images chứa các hình ảnh được sử dụng trong trò chơi
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png"); //Hình ảnh ô trống
        public readonly static ImageSource Body = LoadImage("Body.png"); //Hình ảnh thân rắn
        public readonly static ImageSource Head = LoadImage("Head.png"); //Hình ảnh đầu rắn
        public readonly static ImageSource Food = LoadImage("Food.png"); //Hình ảnh thức ăn
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png"); //Hình ảnh thân rắn khi chết
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png"); //Hình ảnh đầu rắn khi chết

        private static ImageSource LoadImage(string fileName) //Phương thức tải hình ảnh từ tệp
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative)); //Tạo một đối tượng BitmapImage với đường dẫn tệp tương đối
        }
    }
}
