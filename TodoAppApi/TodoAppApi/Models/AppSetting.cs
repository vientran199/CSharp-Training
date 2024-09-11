namespace TodoAppApi.Models
{
    //Tao cai nay de no tu dong map với giá trị trong file appsettings.json, để lấy giá trị dùng ở nhiều chỗ khác
    //Vì giá trị ở appsetting.json chỉ lấy trực tiếp được khi ở file program.cs
    public class AppSetting
    {
        public string SecretKey { get; set; }
    }
}
