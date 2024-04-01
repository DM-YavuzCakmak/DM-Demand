using Demand.Domain.NebimProductModel;
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
        public List<NebimProductsInfoModel> RunSqlQuery()
        {
            List<NebimProductsInfoModel> nebimProductsInfos = new List<NebimProductsInfoModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT ProductCode = cdItem.ItemCode , ProductDescription = ISNULL(ItemDescription, SPACE(0)) ,ProductHierarchyLevel01 , ProductHierarchyLevel02 FROM cdItem WITH(NOLOCK) LEFT OUTER JOIN cdItemDesc WITH(NOLOCK) ON cdItemDesc.ItemTypeCode = cdItem.ItemTypeCode AND cdItemDesc.ItemCode = cdItem.ItemCode AND cdItemDesc.LangCode ='TR' LEFT OUTER JOIN ProductHierarchy('TR') ON cdItem.ProductHierarchyID = ProductHierarchy.ProductHierarchyID WHERE cdItem.ItemTypeCode = 1 AND cdItem.ItemCode <> SPACE(0)"; 

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimProductsInfoModel nebimProductsInfo = new NebimProductsInfoModel();
                                nebimProductsInfo.ProductCode= reader["ProductCode"].IsNotNull() ? reader["ProductCode"].ToString() : "";
                                nebimProductsInfo.productDescription = reader["ProductDescription"].IsNotNull() ? reader["ProductDescription"].ToString() : "";
                                nebimProductsInfo.productHierarchyLevel01 = reader["ProductHierarchyLevel01"].IsNotNull() ? reader["ProductHierarchyLevel01"].ToString() : "";
                                nebimProductsInfo.productHierarchyLevel02 = reader["ProductHierarchyLevel02"].IsNotNull() ? reader["ProductHierarchyLevel02"].ToString() : "";
                                nebimProductsInfos.Add(nebimProductsInfo);    
                            }

                        }
                    }
                }
                return nebimProductsInfos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimProductsInfos;
        }
    }
}
