using Demand.Domain.NebimModels;
using Kep.Helpers.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Core.DatabaseConnection.NebimConnection
{
    public class NebimConnection
    {
        private string connectionString = "Data Source=172.30.196.11;Initial Catalog=Dem_V3;User Id=sa;Password=Asist@1489;TrustServerCertificate=true;";
        public List<NebimCategoryModel> GetNebimCategoryModels()
        {
            List<NebimCategoryModel> nebimCategoryModels = new List<NebimCategoryModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT DISTINCT ISNULL(cphld.ProductHierarchyLevelCode,0) as ProductHierarchyLevel01Code , cphld.ProductHierarchyLevelDescription as ProductHierarchyLevel01Description from cdItem ci  WITH(NOLOCK) LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on ci.ProductHierarchyID = dph.ProductHierarchyID LEFT OUTER join cdProductHierarchyLevelDesc cphld WITH(NOLOCK) on cphld.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode01 WHERE ci.ItemTypeCode = 1 AND ci.ItemCode <> SPACE(0);"; 

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimCategoryModel nebimProductsInfo = new NebimCategoryModel();
                                nebimProductsInfo.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                nebimProductsInfo.ProductHierarchyLevel01Description = reader["ProductHierarchyLevel01Description"].IsNotNull() ? reader["ProductHierarchyLevel01Description"].ToString() : "";
                                nebimCategoryModels.Add(nebimProductsInfo);    
                            }

                        }
                    }
                }
                return nebimCategoryModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimCategoryModels;
        }

        public List<NebimSubCategoryModel> GetNebimSubCategoryModels()
        {
            List<NebimSubCategoryModel> nebimSubCategoryModels = new List<NebimSubCategoryModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT DISTINCT ISNULL(cphld.ProductHierarchyLevelCode,0) as ProductHierarchyLevel02Code, dph.ProductHierarchyLevelCode01 as ProductHierarchyLevel01Code , cphld.ProductHierarchyLevelDescription as ProductHierarchyLevel02Description from cdItem ci  WITH(NOLOCK) LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on ci.ProductHierarchyID = dph.ProductHierarchyID LEFT OUTER join cdProductHierarchyLevelDesc cphld WITH(NOLOCK) on cphld.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode02 WHERE ci.ItemTypeCode = 1 AND ci.ItemCode <> SPACE(0);";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimSubCategoryModel  subCategoryModel = new NebimSubCategoryModel();
                                subCategoryModel.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                subCategoryModel.ProductHierarchyLevel02Code = reader["ProductHierarchyLevel02Code"].IsNotNull() ? reader["ProductHierarchyLevel02Code"].ToString() : "";
                                subCategoryModel.ProductHierarchyLevel02Description = reader["ProductHierarchyLevel02Description"].IsNotNull() ? reader["ProductHierarchyLevel02Description"].ToString() : "";
                                nebimSubCategoryModels.Add(subCategoryModel);
                            }

                        }
                    }
                }
                return nebimSubCategoryModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimSubCategoryModels;
        }


        public List<NebimProductModel> GetNebimProductModels()
        {
            List<NebimProductModel> nebimProductModels = new List<NebimProductModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT ISNULL(cphld1.ProductHierarchyLevelCode, 0) as ProductHierarchyLevel01Code , ISNULL(cphld2.ProductHierarchyLevelCode, 0) as ProductHierarchyLevel02Code, ci.ItemCode as ProductCode , ISNULL(cdItemDesc.ItemDescription,SPACE(0)) as ProductDescription from cdItem ci WITH(NOLOCK) LEFT OUTER JOIN cdItemDesc WITH(NOLOCK) ON cdItemDesc.ItemTypeCode = ci.ItemTypeCode AND cdItemDesc.ItemCode = ci.ItemCode AND cdItemDesc.LangCode = 'TR' LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on ci.ProductHierarchyID = dph.ProductHierarchyID LEFT OUTER join cdProductHierarchyLevelDesc cphld1 WITH(NOLOCK) on cphld1.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode01 LEFT OUTER join cdProductHierarchyLevelDesc cphld2 WITH(NOLOCK) on cphld2.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode02 WHERE ci.ItemTypeCode = 1 AND ci.ItemCode <> SPACE(0);";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimProductModel nebimProductModel = new NebimProductModel();
                                nebimProductModel.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                nebimProductModel.ProductHierarchyLevel02Code = reader["ProductHierarchyLevel02Code"].IsNotNull() ? reader["ProductHierarchyLevel02Code"].ToString() : "";
                                nebimProductModel.ProductCode = reader["ProductCode"].IsNotNull() ? reader["ProductCode"].ToString() : "";
                                nebimProductModel.ProductDescription = reader["ProductDescription"].IsNotNull() ? reader["ProductDescription"].ToString() : "";

                                nebimProductModels.Add(nebimProductModel);
                            }

                        }
                    }
                }
                return nebimProductModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimProductModels;
        }
    }
}
