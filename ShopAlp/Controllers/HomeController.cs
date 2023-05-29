using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ShopAlp.Models;
using ShopAlp.Scripts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

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
                return View("Climbing", dataP);
            }
            else
            {

            }
            return View();

        }

    }
}