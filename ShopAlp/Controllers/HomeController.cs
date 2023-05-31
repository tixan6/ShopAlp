using Messanger.Scripts.HashPasswd;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ShopAlp.Models;
using ShopAlp.Scripts;

namespace ShopAlp.Controllers
{
    public class HomeController : Controller
    {
        ConnectToDatabase connect;
        List<List<object>> productAll;
        List<object> product;
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Pay()
        {
            return View();
        }
        public IActionResult Friends()
        {
            return View();
        }






        //Общий каталог
        public IActionResult Climbing(int pg = 1, string name = "Equipment")
        {

            productAll = new List<List<object>>();


            connect = new ConnectToDatabase();
            connect.Open();
            object data = connect.take($"select * from \"{name}\"");
            if (data is NpgsqlDataReader)
            {
                NpgsqlDataReader dataProduct = (NpgsqlDataReader)data;
                while (dataProduct.Read())
                {
                    product = new List<object>();

                    for (int i = 0; i < dataProduct.VisibleFieldCount; i++)
                    {
                        product.Add(dataProduct.GetValue(i));
                    }
                    productAll.Add(product);
                }
                connect.Close();



                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                int recsCoutn = productAll.Count;
                var pager = new Pager(recsCoutn, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var dataP = productAll.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                this.ViewBag.type = name;
                DataUser.dataP = dataP;
                return View(dataP);
            }
            else
            {

            }
            return View();

        }


        public IActionResult Subdirectory(int pg = 1, string type = "Веревка", string dataBd = "Equipment")
        {

            productAll = new List<List<object>>();

            connect = new ConnectToDatabase();
            connect.Open();
            object data = connect.take($"select * from \"{dataBd}\" WHERE type = '{type}'");
            if (data is NpgsqlDataReader)
            {
                NpgsqlDataReader dataProduct = (NpgsqlDataReader)data;
                while (dataProduct.Read())
                {
                    product = new List<object>();

                    for (int i = 0; i < dataProduct.VisibleFieldCount; i++)
                    {
                        product.Add(dataProduct.GetValue(i));
                    }
                    productAll.Add(product);
                }
                connect.Close();



                const int pageSize = 10;
                if (pg < 1)
                {
                    pg = 1;
                }
                int recsCoutn = productAll.Count;
                var pager = new Pager(recsCoutn, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var dataP = productAll.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;


                this.ViewBag.type = type;
                this.ViewBag.dataBd = dataBd;
                DataUser.dataP = dataP;
                return View("Climbing", dataP);
            }
            else
            {

            }
            return View();

        }







     
        public IActionResult Sigin() 
        {
            return View();
        }
        public IActionResult exit()
        {
            DataUser.onaccoutn = false;
            //Удалить куки
            return View("index");
        }

        [HttpPost]
        public IActionResult Sigin(Login lg)
        {
            if (ModelState.IsValid)
            {
                string pass = string.Empty;
                string hashLoginPass = Hash.HashPassword(lg.password);
                ConnectToDatabase connect = new ConnectToDatabase();
                connect.Open();
                object items = connect.take($"SELECT \"users\".\"password\" FROM \"users\" WHERE \"users\".\"phone\" = '{lg.phone.Trim()}'");

                if (items is NpgsqlDataReader)
                {
                    NpgsqlDataReader itemsReader = (NpgsqlDataReader)items;

                    while (itemsReader.Read())
                    {
                        pass = itemsReader.GetValue(0).ToString();
                    }

                    connect.Close();
                    if (Hash.VerifyHashedPassword(pass, lg.password))
                    {
                        ConnectToDatabase connectTwo = new ConnectToDatabase();
                        connectTwo.Open();
                        object itemsTwo = connectTwo.take($"SELECT \"name\", phone, surname  FROM \"users\" WHERE \"users\".\"phone\" = '{lg.phone.Trim()}'");
                        NpgsqlDataReader itemsReaderTwo = (NpgsqlDataReader)itemsTwo;
                        while (itemsReaderTwo.Read())
                        {
                            DataUser.name = itemsReaderTwo.GetValue(0).ToString();
                            DataUser.phone = itemsReaderTwo.GetValue(1).ToString();
                            DataUser.surname = itemsReaderTwo.GetValue(2).ToString();
                        }

                        connectTwo.Close();
                        DataUser.onaccoutn = true;
                        ViewData["sucLog"] = true;

                        return View("index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пароль или логин неверный");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пароль или логин неверный");
                    return View();
                }
            }
            else
            {
                return View();
            }
            
        }


        public IActionResult Registrations()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registrations(Registration data_reg)
        {
            if (ModelState.IsValid)
            {

                ConnectToDatabase connectionCheck = new ConnectToDatabase();
                connectionCheck.Open();               
                object data = connectionCheck.take($"SELECT * FROM \"users\" WHERE phone = '{data_reg.phone}'");
                if (data is NpgsqlDataReader)
                {
                    ModelState.AddModelError("", "Пользователь с таким номером уже зарегистрирован");
                    connectionCheck.Close();
                    return View();
                }
                else
                {
                    ConnectToDatabase connection = new ConnectToDatabase();
                    connection.Open();
                    connection.put($"INSERT INTO users (name, surname ,\"password\", phone)\r\nVALUES ('{data_reg.name}', '{data_reg.surname}', '{Hash.HashPassword(data_reg.password)}', '{data_reg.phone}');");
                    connection.Close();
                    ViewData["sucReg"] = true;
                    return View("index");
                }
            }
            else 
            { 
                return View();
            }
            
        }

        public IActionResult InCart(int idProduct) 
        {
 
            if (DataUser.onaccoutn)
            {
                if (HttpContext.Request.Cookies["cart"] != null && HttpContext.Request.Cookies["cart"] != string.Empty)
                {
                    string cart = HttpContext.Request.Cookies["cart"];
                    List<int> ints = cart.Split(',').Select(Int32.Parse).ToList();
                    int all = ints.Find(i => i == idProduct);
                    if (all != idProduct)
                    {
                        ints.Add(idProduct);
                        char delim = ',';
                        string str = String.Join(delim, ints);
                        HttpContext.Response.Cookies.Append("cart", str);
                        ViewData["addCart"] = true;
                        DataUser.coutn++;
                        return View("Climbing");
                    }
                    else
                    {
                        ViewData["rep"] = true;
                        return View("Climbing");
                    }
                    

                }
                else 
                {
                    HttpContext.Response.Cookies.Append("cart", idProduct.ToString());
                    ViewData["addCart"] = true;
                    DataUser.coutn++;
                    return View("Climbing");
                    
                }              
            }
            else
            {
                ViewData["NoCart"] = true;
                return View("Climbing");
            }

        }



        public IActionResult OpenCart() 
        {
            if (HttpContext.Request.Cookies["cart"] != null)
            {
                string cart = HttpContext.Request.Cookies["cart"];
                List<int> ints = cart.Split(',').Select(Int32.Parse).ToList();
                productAll = new List<List<object>>();

                ConnectToDatabase connect = new ConnectToDatabase();
                
                for (int i = 0; i < ints.Count; i++)
                {
                    connect.Open();
                    object items = connect.take($"SELECT *\r\nFROM \"Bags\" where id = {ints[i]}\r\nUNION \r\nSELECT *\r\nFROM \"Equipment\" where id = {ints[i]}\r\nUNION\r\nSELECT *\r\nFROM light where id = {ints[i]}\r\nUNION\r\nSELECT *\r\nFROM \"Tents\" where id = {ints[i]}");

                    if (items is NpgsqlDataReader)
                    {
                        NpgsqlDataReader itemsReader = (NpgsqlDataReader)items;

                        while (itemsReader.Read())
                        {
                            product = new List<object>();

                            for (int j = 0; j < itemsReader.VisibleFieldCount; j++)
                            {
                                product.Add(itemsReader.GetValue(j));
                            }
                            productAll.Add(product);                        

                        }
                        connect.Close();
                    }
                }


                DataUser.productCart = productAll;
                return View();

            }
            else
            {
                return View();

            }

        }

        public IActionResult deleteFromCart(int idProduct) 
        {
            if (HttpContext.Request.Cookies["cart"] != null)
            {
                string cart = HttpContext.Request.Cookies["cart"];
                List<int> ints = cart.Split(',').Select(Int32.Parse).ToList();
                ints.Remove(idProduct);


                productAll = new List<List<object>>();

                ConnectToDatabase connect = new ConnectToDatabase();

                for (int i = 0; i < ints.Count; i++)
                {
                    connect.Open();
                    object items = connect.take($"SELECT *\r\nFROM \"Bags\" where id = {ints[i]}\r\nUNION \r\nSELECT *\r\nFROM \"Equipment\" where id = {ints[i]}\r\nUNION\r\nSELECT *\r\nFROM light where id = {ints[i]}\r\nUNION\r\nSELECT *\r\nFROM \"Tents\" where id = {ints[i]}");

                    if (items is NpgsqlDataReader)
                    {
                        NpgsqlDataReader itemsReader = (NpgsqlDataReader)items;

                        while (itemsReader.Read())
                        {
                            product = new List<object>();

                            for (int j = 0; j < itemsReader.VisibleFieldCount; j++)
                            {
                                product.Add(itemsReader.GetValue(j));
                            }
                            productAll.Add(product);

                        }
                        connect.Close();
                    }
                }
         
                char delim = ',';
                string str = String.Join(delim, ints);
                HttpContext.Response.Cookies.Append("cart", str);
                DataUser.coutn--;
                ViewData["RemoveItems"] = true;
                DataUser.productCart = productAll;
                return View("OpenCart");
            }

            else 
            {
                return View("OpenCart");
            }
            
            
        }
        






    }
}