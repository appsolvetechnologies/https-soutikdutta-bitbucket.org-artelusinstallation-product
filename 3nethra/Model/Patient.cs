using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Artelus.Model
{
    public class Patient
    {
        string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public ObservableCollection<PatientEntity> GetAll(string column = null, string searchText = null)
        {
            string sql = "Select * from Patient";
            if (!string.IsNullOrEmpty(searchText))
            {
                if (column != null && column != "p_id")
                    sql = "select * from patient where " + column + " like '%" + searchText + "%';";
                else if (column != null && column == "p_id")
                    sql = "select * from patient where p_id=" + int.Parse(searchText);
            }

            ObservableCollection<PatientEntity> list = new ObservableCollection<PatientEntity>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                PatientEntity obj = new PatientEntity()
                {
                    Id = rdr.GetInt32(0),
                    Nm = rdr.GetString(1),
                    MNm = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    LNm = rdr.GetString(3),
                    NotResident = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    IfResidentOfM = rdr.GetString(5),
                    IcNumber = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6),
                    OtherOption = rdr.IsDBNull(7) == true ? "" : rdr.GetString(7),
                    OthersID = rdr.IsDBNull(8) == true ? "" : rdr.GetString(8),
                    DocNm = rdr.IsDBNull(9) == true ? "" : rdr.GetString(9),
                    HospitalNm = rdr.IsDBNull(10) == true ? "" : rdr.GetString(10),
                    HospitalID = rdr.IsDBNull(11) == true ? "" : rdr.GetString(11),
                    HospitalScreening = rdr.IsDBNull(12) == true ? "" : rdr.GetString(12),
                    Email = rdr.GetString(13),
                    MaritalStatus = rdr.GetString(14) == "Yes" ? "Y" : "N",
                    Age = rdr.GetInt32(15),
                    Sex = rdr.GetString(16),
                    PerAdr = rdr.IsDBNull(17) == true ? "" : rdr.GetString(17),
                    Area = rdr.IsDBNull(18) == true ? "" : rdr.GetString(18),
                    ResidentPh = rdr.IsDBNull(19) == true ? "" : rdr.GetString(19),
                    Mob = rdr.GetString(20),
                    Occupation = rdr.IsDBNull(21) == true ? "" : rdr.GetString(21),
                    WorkingAt = rdr.IsDBNull(22) == true ? "" : rdr.GetString(22),
                    CurrentMedications = rdr.IsDBNull(23) == true ? "" : rdr.GetString(23),
                    LaserTreatment = rdr.IsDBNull(24) == true ? "" : rdr.GetString(24),
                    Cataract = rdr.IsDBNull(25) == true ? "" : rdr.GetString(25),
                    Hypertension = rdr.IsDBNull(26) == true ? "" : rdr.GetString(26),
                    AllergyDrugs = rdr.IsDBNull(27) == true ? "" : rdr.GetString(27),
                    Diabetic = rdr.IsDBNull(28) == true ? "" : rdr.GetString(28),
                    Info = rdr.IsDBNull(29) == true ? "" : rdr.GetString(29),
                    EmergContactNm = rdr.IsDBNull(30) == true ? "" : rdr.GetString(30),
                    EmergPh = rdr.IsDBNull(31) == true ? "" : rdr.GetString(31),
                    StatedConsentPerson = rdr.IsDBNull(32) == true ? "" : rdr.GetString(32),
                    Relation = rdr.IsDBNull(33) == true ? "" : rdr.GetString(33),
                    TermsCondition = rdr.GetString(34),
                    CollectionID = rdr.GetInt32(35),
                    InstallID = rdr.GetGuid(36),
                    MDt = rdr.GetDateTime(37),
                    CDt = rdr.GetDateTime(38),
                    UniqueID = rdr.GetGuid(39),
                    AllergyDrugsDtl = rdr.IsDBNull(40) == true ? "" : rdr.GetString(40),
                    MedicalInsurance = rdr.IsDBNull(41) == true ? "" : rdr.GetString(41),
                    PatientId = rdr.GetInt32(42)

                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

        public string Get(int id)
        {
            var dict = new Dictionary<string, object>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from Patient Where p_id=" + id, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    dict.Add(rdr.GetName(i), rdr.IsDBNull(i) ? null : rdr.GetValue(i));
                }
            }
            _Conn.Close();
            return JsonConvert.SerializeObject(dict);
        }

        public string GetReportJson(int id = 0)
        {
            var list = new List<Dictionary<string, object>>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from PatientReport Where PatientId=" + id, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var dict = new Dictionary<string, object>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    dict.Add(rdr.GetName(i), rdr.IsDBNull(i) ? null : rdr.GetValue(i));
                }
                list.Add(dict);
            }
            _Conn.Close();
            return JsonConvert.SerializeObject(list);
        }

        public string GetReportDataJson(int patientId)
        {
            var list = new List<Dictionary<string, object>>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from ReportData Where PatientId=" + patientId, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var dict = new Dictionary<string, object>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    if (rdr.GetName(i) != "PatientId")
                        dict.Add(rdr.GetName(i), rdr.IsDBNull(i) ? null : rdr.GetValue(i));
                }
                list.Add(dict);
            }
            _Conn.Close();
            return JsonConvert.SerializeObject(list);
        }

        public int Add(PatientEntity model)
        {
            using (var db = new ArtelusDbContext())
            {
                string sql = "INSERT INTO [Patient] ([name],[pMName],[pLName],[notResident],[ifResidentOfM],[IcNumber],[otherOption],[othersID],[doctosName],[hospitalName],[hospitalID],[hospitaScreening],[p_email],[marital_status],[age],[sex],[permanent_address],[area],[phone_res],[mobile],[occupation],[working_at],[currentMedications],[laser_reatment],[have_cataract],[have_hypertension],[allergy_to_drugs],[have_diabetes],[additional_info],[emg_contact_name],[emg_phone],[name_of_the_stated_onsent],[relation_with_patient],[term_conditation],[collection_id],[install_id],[update_at],[create_at],[UniqueID],[allergy_drugs_details],[MedicalInsurance])" +
                            " VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40}); ";
                db.Database.ExecuteSqlCommand(sql, model.Nm, model.MNm, model.LNm, model.NotResident, model.IfResidentOfM, model.IcNumber, model.OtherOption, model.OthersID, model.DocNm, model.HospitalNm, model.HospitalID, model.HospitalScreening, model.Email, model.MaritalStatus == "Married" ? "Yes" : "No", model.Age, model.Sex == "Male" ? "m" : "f", model.PerAdr, model.Area, model.ResidentPh, model.Mob, model.Occupation, model.WorkingAt, model.CurrentMedications, model.LaserTreatment, model.Cataract, model.Hypertension, model.AllergyDrugs, model.Diabetic, model.Info, model.EmergContactNm, model.EmergPh, model.StatedConsentPerson, model.Relation, model.TermsCondition, model.CollectionID, model.InstallID, model.MDt, model.CDt, model.UniqueID, model.AllergyDrugsDtl, model.MedicalInsurance);
                model.Id = db.Database.SqlQuery<int>("SELECT MAX(p_id) FROM Patient").SingleOrDefault();
            }
            return model.Id;
        }

        public void Update(PatientEntity model)
        {
            using (var db = new ArtelusDbContext())
            {
                string sql = "UPDATE [Patient] SET name={1},pMName={2},pLName={3},ifResidentOfM={4},IcNumber={5},otherOption={6},othersID={7},doctosName={8},hospitalName={9},hospitalID={10},hospitaScreening={11},p_email={12},marital_status={13},age={14},[sex]={15},[permanent_address]={16},[area]={17},[phone_res]={18},[mobile]={19},[occupation]={20},[working_at]={21},[currentMedications]={22},[laser_reatment]={23},have_cataract={24},have_hypertension={25},allergy_to_drugs={26},have_diabetes={27},additional_info={28},emg_contact_name={29},emg_phone={30},name_of_the_stated_onsent={31},relation_with_patient={32},update_at={33},allergy_drugs_details={34},MedicalInsurance={35} Where p_id={0}";
                db.Database.ExecuteSqlCommand(sql, model.Id, model.Nm, model.MNm, model.LNm, model.IfResidentOfM, model.IcNumber, model.OtherOption, model.OthersID, model.DocNm, model.HospitalNm, model.HospitalID, model.HospitalScreening, model.Email, model.MaritalStatus == "Married" ? "Yes" : "No", model.Age, model.Sex == "Male" ? "m" : "f", model.PerAdr, model.Area, model.ResidentPh, model.Mob, model.Occupation, model.WorkingAt, model.CurrentMedications, model.LaserTreatment, model.Cataract, model.Hypertension, model.AllergyDrugs, model.Diabetic, model.Info, model.EmergContactNm, model.EmergPh, model.StatedConsentPerson, model.Relation, model.MDt, model.AllergyDrugsDtl, model.MedicalInsurance);
                //string sql = "UPDATE [Patient] SET [name]={0},[pMName]={1},[pLName]={2},[ifResidentOfM]={3},[IcNumber]={4},[otherOption]={5},[othersID]= {6},[doctosName]={7},[hospitalName]={8},[hospitalID]={9},[hospitaScreening]={10},[p_email]={11},[marital_status]={12},[age]={13},[sex]={14},[permanent_address]={15},[area]={16},[phone_res]={17},[mobile]={18},[occupation]={19},[working_at]={20},[currentMedications]={21},[laser_reatment]={22},[have_cataract]={23},[have_hypertension]={24},[allergy_to_drugs]={25},[have_diabetes]={26},[additional_info]={27},[emg_contact_name]={28},[emg_phone]={29},[name_of_the_stated_onsent]= {30},[relation_with_patient]={31},update_at]={32},[allergy_drugs_details]={33} WHERE p_id={34};";
                //db.Database.ExecuteSqlCommand(sql, model.Nm, model.MNm, model.LNm, model.IfResidentOfM, model.IcNumber, model.OtherOption, model.OthersID, model.DocNm, model.HospitalNm, model.HospitalID, model.HospitalScreening, model.Email, model.MaritalStatus =="Married"?"yes":"no", model.Age, model.Sex=="Male"?"m":"f", model.PerAdr, model.Area, model.ResidentPh, model.Mob, model.Occupation, model.WorkingAt, model.CurrentMedications, model.LaserTreatment, model.Cataract, model.Hypertension, model.AllergyDrugs, model.Diabetic, model.Info, model.EmergContactNm, model.EmergPh, model.StatedConsentPerson, model.Relation, model.MDt, model.AllergyDrugsDtl, model.Id);
                db.SaveChanges();
            }
        }

        public int AddReport(PatientReport model)
        {
            int reportId = 0;
            using (var db = new ArtelusDbContext())
            {
                db.Database.ExecuteSqlCommand("INSERT INTO PatientReport(PatientId,Dt,Location,InstallID,UniqueID) VALUES({0},{1},{2},{3},{4})", model.PatientId, model.Dt, model.Location, model.InstallID, model.UniqueID);
                reportId = db.Database.SqlQuery<int>("SELECT MAX(Id) FROM PatientReport").SingleOrDefault();
            }
            return reportId;
        }

        public int AddReportData(int reportId, string mode, string eye, string img, string prediction, string size, int patientId)
        {
            int reportDataId = 0;
            using (var db = new ArtelusDbContext())
            {
                db.Database.ExecuteSqlCommand("INSERT INTO ReportData(PatientReportId,Mode,Img,Eye,Prediction,Size,PatientId) VALUES({0},{1},{2},{3},{4},{5},{6})", reportId, mode, img, eye, prediction, size, patientId);
                db.SaveChanges();
                reportDataId = db.Database.SqlQuery<int>("SELECT MAX(Id) FROM ReportData").SingleOrDefault();
            }
            return reportDataId;
        }

        public void UpdateReportData(int reportDataId, string prediction)
        {
            using (var db = new ArtelusDbContext())
            {
                db.Database.ExecuteSqlCommand("Update ReportData Set Prediction={1} Where Id={0}", reportDataId, prediction);
            }
        }

        public void UpdateSyncStatus(int reportId)
        {
            using (var db = new ArtelusDbContext())
            {
                string sql = "UPDATE [PatientReport] SET Sync=1 Where Id={0}";
                db.Database.ExecuteSqlCommand(sql, reportId);
                db.SaveChanges();
            }
        }

        public void UpdatePatientId(int id,int patientId)
        {
            using (var db = new ArtelusDbContext())
            {
                string sql = "UPDATE [Patient] SET [PatientId]={1} Where p_id={0}";
                db.Database.ExecuteSqlCommand(sql, id,patientId);
                db.SaveChanges();
            }
        }

        public List<PatientReport> GetAllReport(int id)
        {
            using (var db = new ArtelusDbContext())
            {
                return db.Database.SqlQuery<PatientReport>("Select * from PatientReport Where PatientId={0}", id).ToList();
            }
        }

        public PatientReport GetLastestReport(int id)
        {
            using (var db = new ArtelusDbContext())
            {
                return db.Database.SqlQuery<PatientReport>("select top(1)* from patientreport where patientid={0} order by Id desc;", id).FirstOrDefault();
            }
        }

        public List<ReportData> GetPosteriorOSReportData(int reportId, bool isPrediction)
        {
            string sql = string.Format("select * from ReportData where PatientReportId ={0}and Eye = 'OS' order by Id desc;", reportId);
            if (isPrediction)
                sql = string.Format("select * from ReportData where PatientReportId ={0} and Eye = 'OS' and Mode='POSTERIOR_MODE' order by Id desc;", reportId);

            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

        public List<ReportData> GetPosteriorODReportData(int reportId, bool isPrediction)
        {
            string sql = string.Format("select * from ReportData where PatientReportId ={0}and Eye = 'OD' order by Id desc;", reportId);
            if (isPrediction)
                sql = string.Format("select * from ReportData where PatientReportId ={0} and Eye = 'OD' and Mode='POSTERIOR_MODE' order by Id desc;", reportId);

            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

        public List<ReportData> GetOSAnteriorReportData(int reportId, bool isPrediction)
        {
            string sql = string.Format("select * from ReportData where PatientReportId ={0}and Eye = 'OS' order by Id desc;", reportId);
            if (isPrediction)
                sql = string.Format("select * from ReportData where PatientReportId ={0} and Eye = 'OS' and Mode='ANTERIOR_MODE' order by Id desc;", reportId);

            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

        public List<ReportData> GetODAnteriorReportData(int reportId, bool isPrediction)
        {
            string sql = string.Format("select * from ReportData where PatientReportId ={0}and Eye = 'OD' order by Id desc;", reportId);
            if (isPrediction)
                sql = string.Format("select * from ReportData where PatientReportId ={0} and Eye = 'OD' and Mode='ANTERIOR_MODE' order by Id desc;", reportId);

            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

        public List<ReportData> GetAllReportData(int reportId)
        {
            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from ReportData Where PatientReportId=" + reportId, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size=rdr.IsDBNull(6)==true?"":rdr.GetString(6),
                    PatientId=rdr.GetInt32(7)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

        public List<ReportData> GetPosteriorReportData(int reportId)
        {
            string sql = string.Format("select * from ReportData where PatientReportId ={0} and Mode='POSTERIOR_MODE' order by Id desc;", reportId);

            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }
        public List<ReportData> GetAnteriorReportData(int reportId)
        {
            string sql = string.Format("select * from ReportData where PatientReportId ={0} and Mode='ANTERIOR_MODE' order by Id desc;", reportId);

            List<ReportData> list = new List<ReportData>();
            SqlCeConnection _Conn = new SqlCeConnection(conn);
            _Conn.Open();
            SqlCeCommand cmd = new SqlCeCommand(sql, _Conn);
            SqlCeDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ReportData obj = new ReportData()
                {
                    Id = rdr.GetInt32(0),
                    PatientReportId = rdr.GetInt32(1),
                    Mode = rdr.IsDBNull(2) == true ? "" : rdr.GetString(2),
                    Img = rdr.GetString(3),
                    Eye = rdr.IsDBNull(4) == true ? "" : rdr.GetString(4),
                    Prediction = rdr.IsDBNull(5) == true ? "" : rdr.GetString(5),
                    Size = rdr.IsDBNull(6) == true ? "" : rdr.GetString(6)
                };
                list.Add(obj);
            }
            _Conn.Close();
            return list;
        }

    }
}
