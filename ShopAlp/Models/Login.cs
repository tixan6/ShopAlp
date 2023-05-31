using System.ComponentModel.DataAnnotations;

namespace ShopAlp.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [RegularExpression("^\\s*\\+?375((33\\d{7})|(29\\d{7})|(44\\d{7}|)|(25\\d{7}))\\s*$", ErrorMessage = "Номер введен некорректно")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
