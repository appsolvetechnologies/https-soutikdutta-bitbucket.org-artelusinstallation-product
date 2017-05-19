using Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.Model
{
    public class User
    {
        string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public UserEntity Validate(string userNm, string pwd)
        {
            UserEntity model = new UserEntity();
            string sql = string.Format("SELECT *  FROM [Users] where usernm='{0}' and pwd='{1}';", userNm, pwd);

            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {

                model.Id = rdr.GetInt32(0);
                model.UserNm = rdr.GetString(1);
                model.Pwd = rdr.GetString(2);
                model.IsConfigured = rdr.GetBoolean(3);
                if (!rdr.IsDBNull(4))
                    model.InstallID = rdr.GetGuid(4);
                model.Location = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5);
                model.PinCode = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6);
            }
            _Conn.Close();
            return model;
        }

        public IdNameCollection GetLocations()
        {
            var locations = new List<Location>();
            IdNameCollection locationColl = new IdNameCollection();
            using (var db = new ArtelusDbContext())
            {
                locations = db.Database.SqlQuery<Location>("Select * From Location").ToList();
            }
            foreach (var loc in locations)
                locationColl.Add(new IdName() { Id = loc.Id, Name = loc.Nm + " - " + loc.PinCode });

            return locationColl;
        }

        public bool Update(UserEntity user)
        {
            int count = 0;
            using (var db = new ArtelusDbContext())
            {
                count = db.Database.ExecuteSqlCommand("Update [Users] SET IsConfigured={0},InstallID={1},Location={2},PinCode={3} Where Id={4}", user.IsConfigured, user.InstallID, user.Location, user.PinCode, user.Id);
                db.SaveChanges();
            }
            return count > 0;
        }

        public Guid GetInstallID(int id)
        {
            using (var db = new ArtelusDbContext())
            {
              return  db.Database.SqlQuery<Guid>("Select InstallID From Users Where Id={0}", id).Single(); ;
            }
        }
    }
}
