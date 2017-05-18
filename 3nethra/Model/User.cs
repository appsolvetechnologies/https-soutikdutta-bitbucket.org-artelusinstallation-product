using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.Model
{
    public class User
    {
        public UserEntity Validate(string userNm, string pwd)
        {
            UserEntity model = new UserEntity();
            using (var db = new ArtelusDbContext())
            {
                model = db.Database.SqlQuery<UserEntity>("Select * From Users Where UserNm={0} AND Pwd={1}", userNm, pwd).FirstOrDefault();
            }
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
    }
}
