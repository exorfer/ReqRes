using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using xNet;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string link = "https://reqres.in/api/users/";// строка со ссылкой на сервер 
        int control = 1;//глобальная паременная-счетчик
        const int per_page = 3;//количество элементов на странице

        public dynamic json(string apilink)//для облегчения чтения кода вывел запрос на сервер отдельной функцией
        {
            try
            {
                var reqres = new HttpRequest();
                HttpResponse response = reqres.Get(apilink);
                return JObject.Parse(response.ToString());
            }
            catch
            {
                label1.Text = "Ошибка при загрузке данных";
                return 0;
            }
        }

        public void download (int point)//функция для получения пользователей от сервера
        {

            int total = json(link).total;// получаю общее количество пользователей
            int i = new int();
            for (i = point; i < point+per_page; i++)//цикл, реализующий постраничный вывод
            {
                if (i < 1)
                {
                    button1.Enabled = false;
                    break;
                }
                link = link + i.ToString();
                dynamic data = json(link);//парсинг пользователей выполняю при помощи JSON. 
                string user = data.data.first_name + " " + data.data.last_name;
                link = link.Remove(28, i.ToString().Length);
                listBox1.Items.Add(user);
                //проверяю, не выйдет ли параметр цикла за общее количество пользователей во время выполнения. Если выходит - выхожу из цикла и отключаю кнопку далее
                if (i >= total)
                {
                    button2.Enabled = false;
                    break;
                }
                if (i + per_page > total)
                    button2.Enabled = false;                    
            }
        }

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            label1.Text = "Статус выполнения: идет обработка обработка данных";
            download(control);
            label1.Text = "Статус выполнения: обработка завершена";
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = true;//при нажатии на кнопку "Далее" активируется кнопка "Назад", и наоборот
            listBox1.Items.Clear();
            label1.Text = "Статус выполнения: идет обработка данных";
            control = control + per_page;
            download(control);
            label1.Text = "Статус выполнения: обработка завершена";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            label1.Text = "Статус выполнения: идет обработка данных";
            listBox1.Items.Clear();
            control = control - per_page;
            download(control);
            if (control <= per_page) button1.Enabled = false;//если программа доберется до первой группы пользователей, кнопка деактивируется
            label1.Text = "Статус выполнения: обработка завершена";
        }
    }
}