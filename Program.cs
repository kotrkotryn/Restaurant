using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Restaurant
{
    class Program
    {
        private readonly IJsonReader _reader;
        public Program(IJsonReader reader)
        {
            _reader = reader;
        }

        static void Main(string[] args)
        {
            JsonReader service = new JsonReader();
            var drinks = service.ReturnDrinkItems().Food;
            var food = service.ReturnFoodItems().Food;
            Order currentOrder = new Order();
            List<Order> orders = new List<Order>();
            List<TableModel> _tables = new List<TableModel>()
        {
            new TableModel(){ Id = 0, Name = "Window Table", NumberOfSeats = 2, Occupied = false },
            new TableModel(){ Id = 1, Name = "Entrance Table", NumberOfSeats = 3, Occupied = false},
            new TableModel(){ Id = 2, Name = "Middle Table", NumberOfSeats = 6, Occupied = false }
        };

            while (true)
            {
                Console.WriteLine("What would you like to to, specify a command number");
                Console.WriteLine("1. Select a table");
                Console.WriteLine("2. Take a food order");
                Console.WriteLine("3. Free up a table");
                Console.WriteLine("4. Print a check for the customer");
                Console.WriteLine("5. Todays orders and amount of money made FOR RESTAUNANT USE ONLY");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Please select a table where to sit:");
                        foreach (var table in _tables.Where(x => x.Occupied == false))
                        {
                            Console.WriteLine($"Table name - {table.Name}, available seats - {table.NumberOfSeats}");
                        }
                        while (input != "Step complete")
                        {
                            input = Console.ReadLine();
                            if (!_tables.Select(x => x.Name).Contains(input))
                            {
                                Console.WriteLine("Specified table does not exist, please choose one from the list:");
                                foreach (var table in _tables.Where(x => x.Occupied == false))
                                {
                                    Console.WriteLine($"Table name - {table.Name}, available seats - {table.NumberOfSeats}");
                                }
                            }
                            if (_tables.Select(x => x.Name).Contains(input))
                            {
                                _tables.FirstOrDefault(x => x.Name == input).Occupied = true;
                                currentOrder.OrderDate = DateTime.Now;
                                currentOrder.Id = orders.Count();
                                currentOrder.Table = input;
                                Console.WriteLine($"---------> {input} booked --------->");
                                input = "Step complete";
                            };
                        }
                        break;
                    case "2":
                        Console.WriteLine("Please insert an order:");
                        while (true)
                        {
                            bool sucess = false;
                            foreach (var drink in drinks)
                            {
                                Console.WriteLine($"Drink name - {drink.Name}, price - {drink.Price}");
                            }
                            foreach (var foodItem in food)
                            {
                                Console.WriteLine($"Food name - {foodItem.Name}, price - {foodItem.Price}");
                            }
                            input = Console.ReadLine();
                            while (sucess != true)
                            {
                                if (drinks.Select(x => x.Name).Contains(input))
                                {
                                    sucess = true;
                                    currentOrder.FoodDrink.Add(drinks.FirstOrDefault(x => x.Name == input));
                                }
                                if (food.Select(x => x.Name).Contains(input))
                                {
                                    sucess = true;
                                    currentOrder.FoodDrink.Add(food.FirstOrDefault(x => x.Name == input));
                                }
                                else
                                {
                                    Console.WriteLine("Sorry we do not have this product, please select from one above");
                                    input = Console.ReadLine();
                                };

                            }
                            Console.WriteLine("Would there be anything else? Yes/No");
                            input = Console.ReadLine();

                            if (input == "Yes")
                            {
                                continue;
                            }
                            if (input == "No")
                            {
                                orders.Add(currentOrder);
                                break;
                            }
                        }
                        break;
                    case "3":
                        while (input != "Step Complete")
                        {
                            if (_tables.Where(x => x.Occupied == true).Count() == 0)
                            {
                                Console.WriteLine("No tables are occupied");
                                input = "Step Complete";
                                break;
                            }
                            foreach (var table in _tables.Where(x => x.Occupied == true))
                            {
                                Console.WriteLine($"Table name - {table.Name}, table number - {table.Id}");
                            }
                            Console.WriteLine("Please input the number of the table you want to free up:");
                            input = Console.ReadLine();
                            if (!string.IsNullOrEmpty(input) && _tables.Where(x => x.Occupied == true).Select(x => x.Id).ToList().Contains(int.Parse(input)))
                            {
                                _tables.FirstOrDefault(x => x.Id == int.Parse(input)).Occupied = false;
                                Console.WriteLine($"Table number {input} is now free");
                                input = "Step Complete";
                            }
                            else
                            {
                                Console.WriteLine("Such table does not exist, please choose from one above");
                            }
                        }
                        break;
                    case "4":
                        while (true)
                        {
                            Console.WriteLine("Would you like a printed receipt? Yes/No");
                            input = Console.ReadLine();
                            if (input == "Yes")
                            {
                                Console.WriteLine("Please enter the order number for which you want a printed receipt:");
                                foreach (var order in orders)
                                {
                                    Console.WriteLine($"Table name - {order.Table}, order number - {order.Id}");
                                }
                                input = Console.ReadLine();
                                if (orders.Select(x => x.Id.ToString()).ToList().Contains(input))
                                {
                                    var order = orders.FirstOrDefault(x => x.Id.ToString() == input);
                                    order.AmountToPay = order.FoodDrink.Select(x => Decimal.Parse(x.Price)).Sum();
                                    foreach (var foodItem in orders.FirstOrDefault(x => x.Id.ToString() == input).FoodDrink)
                                    {
                                        Console.WriteLine($"Item - {foodItem.Name}, price {foodItem.Price}");
                                    }
                                    Console.WriteLine($"The total amount to pay is - {order.AmountToPay}");
                                    orders.Remove(order);
                                    service.CreateOrderFile(order);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("No such order exists, exiting the receipt formation");
                                    break;
                                }
                            }
                            if (input == "No")
                            {
                                Console.WriteLine("Please enter the order number for which you want a receipt:");
                                foreach (var orderItem in orders)
                                {
                                    Console.WriteLine($"Table name - {orderItem.Table}, order number - {orderItem.Id}");
                                }
                                input = Console.ReadLine();
                                if (orders.Select(x => x.Id.ToString()).ToList().Contains(input))
                                {
                                    var order = orders.FirstOrDefault(x => x.Id.ToString() == input);
                                    orders.Remove(order);
                                    service.CreateOrderFile(order);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("No such order exists, exiting the receipt formation");
                                    break;
                                }
                            }
                        }
                        break;
                    case "5":
                        Console.WriteLine($"Date {DateTime.Today}");
                        decimal TotalAmount = 0;
                        foreach (var ordersList in orders)
                        {
                            Console.WriteLine($"Orders - {ordersList.AmountToPay}");
                            TotalAmount += ordersList.AmountToPay;
                            Console.WriteLine($"Total amount: {TotalAmount}");
                        }
                        break;
                    default:
                        break;
                }
            }
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("username", "password"),
                EnableSsl = true,
            };
            smtpClient.Send("email", "recipient", "Restaurant", "TodaysOrders");


            var mailMessage = new MailMessage
            {
                From = new MailAddress("email"),
                Subject = "Restaurant",
                Body = "<h1>TodaysOrders</h1>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add("recipient");

            smtpClient.Send(mailMessage);

            var attachment = new Attachment("C:/Users/Kotryn/Desktop/Restaurant/ReceiptFolder/TodaysOrders.json", MediaTypeNames.Text.Plain);
            mailMessage.Attachments.Add(attachment);
        }

    }
}