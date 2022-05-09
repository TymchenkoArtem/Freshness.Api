using System;
using System.Collections.Generic;

namespace Freshness.Models.ResponseModels
{
    public class OrderResponseModel
    {
        public int Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string DeliveryTime { get; set; }

        public DateTime AddedDate { get; set; }

        public int Amount { get; set; }

        public string Container { get; set; }

        public int TotalCost { get; set; }

        public bool IsDone { get; set; }

        public DateTime IsDoneDate { get; set; }

        public WorkerResponseModel Worker { get; set; }

        public string Note { get; set; }

        public CustomerResponseModel Customer { get; set; }

        public List<AccessoryResponseModel> Accessories { get; set; }

        public override string ToString()
        {
            var order = "# # # # # START ORDER # # # # #\n\n";

            order += $"Адрес: {Customer.Address.ToString()}\n" +
                $"Телефон: +38{Customer.Phone}\n" +
                $"Дата: {DeliveryDate.ToString("dd.MM.yyyy")}\n" +
                $"Время: {DeliveryTime}\n" +
                $"Количество: {Amount}\n" +
                $"Тара: {Container}\n";

            if (Note.Length > 0)
            {
                order += $"Заметка: {Note}\n";
            }

            order += "\n";

            Accessories.ForEach(accessory =>
            {
                order += $" - {accessory.Name}\n";
            });

            order += $"\nИтого: {TotalCost} грн.";

            order += "\n\n# # # # # END ORDER # # # # # # ";

            return order;
        }
    }
}
