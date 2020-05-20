using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EstateAgency
{
    public class AccountRepository
    {
        public AccountRepository()
        {

        }

        // хэштрование паролей
        private string salt = "gt#90mCti2.q";

        static string GetHash(string password, string salt) //Получение хэш-значения
        {
            MD5 md5 = new MD5CryptoServiceProvider(); //Экземпляр объекта MD5
            byte[] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt)); //Вычисление хэш-значения
            string base64digest = Convert.ToBase64String(digest, 0, digest.Length); //Получение строкового значения из массива байт
            return base64digest;
        }

        // авторизация пользователей
        public Client LoginClient(string Phone, string Password)
        {
            Password = GetHash(Password, salt);
            Client user = null;
            using (Agency db = new Agency())
            {
                user = db.Clients.FirstOrDefault(u => u.Phone == Phone && u.Password == Password);
            }
            return user;
        }

        public Manager LoginManager(string Phone, string Password)
        {
            Password = GetHash(Password, salt);
            Manager user = null;
            using (Agency db = new Agency())
            {
                user = db.Managers.FirstOrDefault(u => u.Phone == Phone && u.Password == Password);
            }
            return user;
        }

        // проверка пользователя с данным логином в бд
        public bool ClientExist(string Phone)
        {
            using (Agency db = new Agency())
            {
                Client user = db.Clients.FirstOrDefault(u => u.Phone == Phone);
                if (user != null)
                    return true;
                else return false;
            }
        }

        public bool ManagerExist(string Phone)
        {
            using (Agency db = new Agency())
            {
                Manager user = db.Managers.FirstOrDefault(u => u.Phone == Phone);
                if (user != null)
                    return true;
                else return false;
            }
        }

        // добавление пользователя в бд
        public async Task <Client> CreateClient(string Email, string Password, string Phone, string Surname, string Name, string Patronymic)
        {
            Client user = new Client();
            Password = GetHash(Password, salt);
            using (Agency db = new Agency())
            {
                db.Clients.Add(new Client { Email = Email, Password = Password, Phone = Phone, Surname = Surname, Name = Name, Patronymic = Patronymic });
                await db.SaveChangesAsync();

                user = db.Clients.Where(u => u.Phone == Phone && u.Password == Password).FirstOrDefault();
            }
            return user;
        }

        public async Task<Manager> CreateManager(string Email, string Password, string Phone, string Surname, string Name, string Patronymic)
        {
            Manager user = new Manager();
            Password = GetHash(Password, salt);
            using (Agency db = new Agency())
            {
                db.Managers.Add(new Manager { Email = Email, Password = Password, Phone = Phone, Surname = Surname, Name = Name, Patronymic = Patronymic });
                await db.SaveChangesAsync();

                user = db.Managers.Where(u => u.Phone == Phone && u.Password == Password).FirstOrDefault();
            }
            return user;
        }
    }
}