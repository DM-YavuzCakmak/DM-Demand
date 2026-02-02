using Demand.Domain.NebimModels;
using Kep.Helpers.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Demand.Core.DatabaseConnection.NebimConnection
{
    public class NebimConnection
    {
        private string connectionStringDEM = "Data Source=172.30.196.11;Initial Catalog=Dem_V3;User Id=sa;Password=Asist@1489;TrustServerCertificate=true;";
        private string connectionStringKEP = "Data Source=172.30.196.11;Initial Catalog=KEP_V3;User Id=sa;Password=Asist@1489;TrustServerCertificate=true;";

        public List<NebimCategoryModel> GetNebimCategoryModels()
        {
            List<NebimCategoryModel> nebimCategoryModels = new List<NebimCategoryModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringDEM))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT
                                        	DISTINCT ISNULL(cphld.ProductHierarchyLevelCode, 0) as ProductHierarchyLevel01Code ,
                                        	cphld.ProductHierarchyLevelDescription as ProductHierarchyLevel01Description,
                                        	(case when prItemCompanyBrand.CompanyBrandCode = 'AstelH' then 1 else 0 end) as CompanyBrandCode,
                                        	'1' as CompanyCode
                                        from
                                        	cdItem ci WITH(NOLOCK)
                                        LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on
                                        	ci.ProductHierarchyID = dph.ProductHierarchyID
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld WITH(NOLOCK) on
                                        	cphld.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode01
                                        inner join prItemCompanyBrand with (nolock)
                                        					   on prItemCompanyBrand.ItemTypeCode=ci.ItemTypeCode 
                                        					   and prItemCompanyBrand.ItemCode=ci.ItemCode
                                        					   and prItemCompanyBrand.CompanyBrandCode in ('AstelH','AstelR')
                                        WHERE
                                        	ci.ItemTypeCode = 1
                                        	and ci.ProductTypeCode=1
                                        	AND ci.ItemCode <> SPACE(0)
                                        	   and ci.ItemCode not in ('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15');"; 

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimCategoryModel nebimProductsInfo = new NebimCategoryModel();
                                nebimProductsInfo.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                nebimProductsInfo.ProductHierarchyLevel01Description = reader["ProductHierarchyLevel01Description"].IsNotNull() ? reader["ProductHierarchyLevel01Description"].ToString() : "";
                                nebimProductsInfo.CompanyBrandCode = reader["CompanyBrandCode"].IsNotNull() ? reader["CompanyBrandCode"].ToString() : "";
                                nebimProductsInfo.CompanyCode = reader["CompanyCode"].IsNotNull() ? reader["CompanyCode"].ToString() : "";
                                nebimCategoryModels.Add(nebimProductsInfo);    
                            }

                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionStringKEP))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT
                                        	DISTINCT ISNULL(cphld.ProductHierarchyLevelCode, 0) as ProductHierarchyLevel01Code ,
                                        	cphld.ProductHierarchyLevelDescription as ProductHierarchyLevel01Description,
                                        	(case when prItemCompanyBrand.CompanyBrandCode = 'AH' then 1 else 0 end) as CompanyBrandCode,
                                        	'2' as CompanyCode
                                        from
                                        	cdItem ci WITH(NOLOCK)
                                        LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on
                                        	ci.ProductHierarchyID = dph.ProductHierarchyID
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld WITH(NOLOCK) on
                                        	cphld.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode01
                                        inner join prItemCompanyBrand with (nolock)
                                        					   on prItemCompanyBrand.ItemTypeCode=ci.ItemTypeCode 
                                        					   and prItemCompanyBrand.ItemCode=ci.ItemCode
                                        					   and prItemCompanyBrand.CompanyBrandCode in ('AH','AR')
                                        WHERE
                                        	ci.ItemTypeCode = 1
                                        	and ci.ProductTypeCode=1
                                        	AND ci.ItemCode <> SPACE(0)
                                        	   and ci.ItemCode not in ('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15');"; 

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimCategoryModel nebimProductsInfo = new NebimCategoryModel();
                                nebimProductsInfo.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                nebimProductsInfo.ProductHierarchyLevel01Description = reader["ProductHierarchyLevel01Description"].IsNotNull() ? reader["ProductHierarchyLevel01Description"].ToString() : "";
                                nebimProductsInfo.CompanyBrandCode = reader["CompanyBrandCode"].IsNotNull() ? reader["CompanyBrandCode"].ToString() : "";
                                nebimProductsInfo.CompanyCode = reader["CompanyCode"].IsNotNull() ? reader["CompanyCode"].ToString() : "";
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
                using (SqlConnection connection = new SqlConnection(connectionStringDEM))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT
                                        	DISTINCT ISNULL(cphld.ProductHierarchyLevelCode,
                                        	0) as ProductHierarchyLevel02Code,
                                        	dph.ProductHierarchyLevelCode01 as ProductHierarchyLevel01Code ,
                                        	cphld.ProductHierarchyLevelDescription as ProductHierarchyLevel02Description,
                                        	(case when prItemCompanyBrand.CompanyBrandCode = 'AstelH' then 1 else 0 end) as CompanyBrandCode,
                                        	'1' as CompanyCode
                                        from
                                        	cdItem ci WITH(NOLOCK)
                                        LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on
                                        	ci.ProductHierarchyID = dph.ProductHierarchyID
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld WITH(NOLOCK) on
                                        	cphld.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode02
                                        inner join prItemCompanyBrand with (nolock)
                                        					   on prItemCompanyBrand.ItemTypeCode=ci.ItemTypeCode 
                                        					   and prItemCompanyBrand.ItemCode=ci.ItemCode
                                        					   and prItemCompanyBrand.CompanyBrandCode in ('AstelH','AstelR')
                                        WHERE
                                        	ci.ItemTypeCode = 1
                                        	and ci.ProductTypeCode=1
                                        	AND ci.ItemCode <> SPACE(0)
                                        	   and ci.ItemCode not in ('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15');";

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
                                subCategoryModel.CompanyBrandCode = reader["CompanyBrandCode"].IsNotNull() ? reader["CompanyBrandCode"].ToString() : "";
                                subCategoryModel.CompanyCode = reader["CompanyCode"].IsNotNull() ? reader["CompanyCode"].ToString() : "";
                                nebimSubCategoryModels.Add(subCategoryModel);
                            }

                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionStringKEP))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT
                                        	DISTINCT ISNULL(cphld.ProductHierarchyLevelCode,
                                        	0) as ProductHierarchyLevel02Code,
                                        	dph.ProductHierarchyLevelCode01 as ProductHierarchyLevel01Code ,
                                        	cphld.ProductHierarchyLevelDescription as ProductHierarchyLevel02Description,
                                        	(case when prItemCompanyBrand.CompanyBrandCode = 'AH' then 1 else 0 end) as CompanyBrandCode,
                                        	'2' as CompanyCode
                                        from
                                        	cdItem ci WITH(NOLOCK)
                                        LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on
                                        	ci.ProductHierarchyID = dph.ProductHierarchyID
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld WITH(NOLOCK) on
                                        	cphld.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode02
                                        inner join prItemCompanyBrand with (nolock)
                                        					   on prItemCompanyBrand.ItemTypeCode=ci.ItemTypeCode 
                                        					   and prItemCompanyBrand.ItemCode=ci.ItemCode
                                        					   and prItemCompanyBrand.CompanyBrandCode in ('AH','AR')
                                        WHERE
                                        	ci.ItemTypeCode = 1
                                        	and ci.ProductTypeCode=1
                                        	AND ci.ItemCode <> SPACE(0)
                                        	   and ci.ItemCode not in ('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15');";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimSubCategoryModel subCategoryModel = new NebimSubCategoryModel();
                                subCategoryModel.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                subCategoryModel.ProductHierarchyLevel02Code = reader["ProductHierarchyLevel02Code"].IsNotNull() ? reader["ProductHierarchyLevel02Code"].ToString() : "";
                                subCategoryModel.ProductHierarchyLevel02Description = reader["ProductHierarchyLevel02Description"].IsNotNull() ? reader["ProductHierarchyLevel02Description"].ToString() : "";
                                subCategoryModel.CompanyBrandCode = reader["CompanyBrandCode"].IsNotNull() ? reader["CompanyBrandCode"].ToString() : "";
                                subCategoryModel.CompanyCode = reader["CompanyCode"].IsNotNull() ? reader["CompanyCode"].ToString() : "";
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
                using (SqlConnection connection = new SqlConnection(connectionStringDEM))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT
                                        	ISNULL(cphld1.ProductHierarchyLevelCode,
                                        	0) as ProductHierarchyLevel01Code ,
                                            cphld1.ProductHierarchyLevelDescription as ProductHierarchyLevel01Description,
                                        	ISNULL(cphld2.ProductHierarchyLevelCode,
                                        	0) as ProductHierarchyLevel02Code,
                                            cphld2.ProductHierarchyLevelDescription as ProductHierarchyLevel02Description,
                                        	ci.ItemCode as ProductCode ,
                                        	ISNULL(cdItemDesc.ItemDescription,
                                        	SPACE(0)) as ProductDescription,
                                        	(case when prItemCompanyBrand.CompanyBrandCode = 'AstelH' then 1 else 0 end) as CompanyBrandCode,
                                        	'1' as CompanyCode,
                                            ci.ItemTaxGrCode
                                        from
                                        	cdItem ci WITH(NOLOCK)
                                        LEFT OUTER JOIN cdItemDesc WITH(NOLOCK) ON
                                        	cdItemDesc.ItemTypeCode = ci.ItemTypeCode
                                        	AND cdItemDesc.ItemCode = ci.ItemCode
                                        	AND cdItemDesc.LangCode = 'TR'
                                        LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on
                                        	ci.ProductHierarchyID = dph.ProductHierarchyID
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld1 WITH(NOLOCK) on
                                        	cphld1.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode01
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld2 WITH(NOLOCK) on
                                        	cphld2.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode02
                                        inner join prItemCompanyBrand with (nolock)
                                        					   on prItemCompanyBrand.ItemTypeCode=ci.ItemTypeCode 
                                        					   and prItemCompanyBrand.ItemCode=ci.ItemCode
                                        					   and prItemCompanyBrand.CompanyBrandCode in ('AstelH','AstelR')
                                        WHERE
                                        	ci.ItemTypeCode = 1
                                        	and ci.ProductTypeCode=1
                                        	AND ci.ItemCode <> SPACE(0)
                                        	   and ci.ItemCode not in ('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15');";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimProductModel nebimProductModel = new NebimProductModel();
                                nebimProductModel.CompanyName = "DEM";
                                nebimProductModel.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                nebimProductModel.ProductHierarchyLevel01Description = reader["ProductHierarchyLevel01Description"].IsNotNull() ? reader["ProductHierarchyLevel01Description"].ToString() : "";
                                nebimProductModel.ProductHierarchyLevel02Code = reader["ProductHierarchyLevel02Code"].IsNotNull() ? reader["ProductHierarchyLevel02Code"].ToString() : "";
                                nebimProductModel.ProductHierarchyLevel02Description = reader["ProductHierarchyLevel02Description"].IsNotNull() ? reader["ProductHierarchyLevel02Description"].ToString() : "";
                                nebimProductModel.CompanyBrandCode = reader["CompanyBrandCode"].IsNotNull() ? reader["CompanyBrandCode"].ToString() : "";
                                nebimProductModel.CompanyCode = reader["CompanyCode"].IsNotNull() ? reader["CompanyCode"].ToString() : "";
                                nebimProductModel.ProductCode = reader["ProductCode"].IsNotNull() ? reader["ProductCode"].ToString() : "";
                                nebimProductModel.ProductDescription = reader["ProductDescription"].IsNotNull() ? reader["ProductDescription"].ToString() : "";
                                nebimProductModel.ItemTaxGrCode = reader["ItemTaxGrCode"].IsNotNull() ? reader["ItemTaxGrCode"].ToString() : "";
                                nebimProductModels.Add(nebimProductModel);
                            }

                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionStringKEP))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT
                                        	ISNULL(cphld1.ProductHierarchyLevelCode,
                                        	0) as ProductHierarchyLevel01Code ,
                                            cphld1.ProductHierarchyLevelDescription as ProductHierarchyLevel01Description,
                                        	ISNULL(cphld2.ProductHierarchyLevelCode,
                                        	0) as ProductHierarchyLevel02Code,
                                            cphld2.ProductHierarchyLevelDescription as ProductHierarchyLevel02Description,
                                        	ci.ItemCode as ProductCode ,
                                        	ISNULL(cdItemDesc.ItemDescription,
                                        	SPACE(0)) as ProductDescription,
                                        	(case when prItemCompanyBrand.CompanyBrandCode = 'AH' then 1 else 0 end) as CompanyBrandCode,
                                        	'2' as CompanyCode,
                                            ci.ItemTaxGrCode
                                        from
                                        	cdItem ci WITH(NOLOCK)
                                        LEFT OUTER JOIN cdItemDesc WITH(NOLOCK) ON
                                        	cdItemDesc.ItemTypeCode = ci.ItemTypeCode
                                        	AND cdItemDesc.ItemCode = ci.ItemCode
                                        	AND cdItemDesc.LangCode = 'TR'
                                        LEFT OUTER join dfProductHierarchy dph WITH(NOLOCK) on
                                        	ci.ProductHierarchyID = dph.ProductHierarchyID
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld1 WITH(NOLOCK) on
                                        	cphld1.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode01
                                        LEFT OUTER join cdProductHierarchyLevelDesc cphld2 WITH(NOLOCK) on
                                        	cphld2.ProductHierarchyLevelCode = dph.ProductHierarchyLevelCode02
                                        inner join prItemCompanyBrand with (nolock)
                                        					   on prItemCompanyBrand.ItemTypeCode=ci.ItemTypeCode 
                                        					   and prItemCompanyBrand.ItemCode=ci.ItemCode
                                        					   and prItemCompanyBrand.CompanyBrandCode in ('AH','AR')
                                        WHERE
                                        	ci.ItemTypeCode = 1
                                        	and ci.ProductTypeCode=1
                                        	AND ci.ItemCode <> SPACE(0)
                                        	   and ci.ItemCode not in ('1','2','3','4','5','6','7','8','9','10','11','12','13','14','15');";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NebimProductModel nebimProductModel = new NebimProductModel();
                                nebimProductModel.CompanyName = "KEP";
                                nebimProductModel.ProductHierarchyLevel01Code = reader["ProductHierarchyLevel01Code"].IsNotNull() ? reader["ProductHierarchyLevel01Code"].ToString() : "";
                                nebimProductModel.ProductHierarchyLevel02Code = reader["ProductHierarchyLevel02Code"].IsNotNull() ? reader["ProductHierarchyLevel02Code"].ToString() : "";
                                nebimProductModel.CompanyBrandCode = reader["CompanyBrandCode"].IsNotNull() ? reader["CompanyBrandCode"].ToString() : "";
                                nebimProductModel.CompanyCode = reader["CompanyCode"].IsNotNull() ? reader["CompanyCode"].ToString() : "";
                                nebimProductModel.ProductCode = reader["ProductCode"].IsNotNull() ? reader["ProductCode"].ToString() : "";
                                nebimProductModel.ProductDescription = reader["ProductDescription"].IsNotNull() ? reader["ProductDescription"].ToString() : "";
                                nebimProductModel.ItemTaxGrCode = reader["ItemTaxGrCode"].IsNotNull() ? reader["ItemTaxGrCode"].ToString() : "";

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

        //Şirket Ofisleri
        public List<NebimOfficeModel> GetOfficeList(int companyId)
        {
            List<NebimOfficeModel> nebimOfficeModels = new();
            try
            {
                using SqlConnection connection = new(companyId == 1 ? connectionStringDEM : connectionStringKEP);
                using SqlCommand command = new("usp_Entegrasyon_OfficeList", connection);

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var invoice = new NebimOfficeModel
                    {
                        CompanyName = "DEM",
                        OfficeCode = reader["OfficeCode"]?.ToString(),
                        OfficeDescription = reader["OfficeDescription"]?.ToString()
                    };

                    nebimOfficeModels.Add(invoice);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimOfficeModels;
        }

        //Depolar
        public List<NebimWarehouseModel> GetWareHouseList(int companyId)
        {
            List<NebimWarehouseModel> nebimWarehouseModels = new();
            try
            {
                using SqlConnection connection = new(companyId == 1 ? connectionStringDEM : connectionStringKEP);
                using SqlCommand command = new("usp_Entegrasyon_WareHouseList", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var warehouseModel = new NebimWarehouseModel
                    {
                        CompanyName = "DEM",
                        OfficeCode = reader["OfficeCode"]?.ToString(),
                        OfficeDescription = reader["OfficeDescription"]?.ToString(),
                        WarehouseCode = reader["WarehouseCode"]?.ToString(),
                        WarehouseDescription = reader["WarehouseDescription"]?.ToString()
                    };

                    nebimWarehouseModels.Add(warehouseModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimWarehouseModels.ToList();
        }

        //Maliyet Merkezleri
        public List<NebimCostModel> GetCostList(int companyId)
        {
            List<NebimCostModel> nebimCostModels = new();
            try
            {
                using SqlConnection connection = new(companyId == 1 ? connectionStringDEM : connectionStringKEP);
                using SqlCommand command = new("usp_Entegrasyon_CostList", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var costModel = new NebimCostModel
                    {
                        CostCenterCode = reader["CostCenterCode"]?.ToString(),
                        CostCenterDescription = reader["CostCenterDescription"]?.ToString()
                    };

                    nebimCostModels.Add(costModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimCostModels.ToList();
        }

        //Masraf Kartları
        public List<NebimExpenseModel> GetExpenseList(int companyId)
        {
            List<NebimExpenseModel> nebimExpenseModels = new();
            try
            {
                using SqlConnection connection = new(companyId == 1 ? connectionStringDEM : connectionStringKEP);
                using SqlCommand command = new("usp_Entegrasyon_ExpenseList", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var invoice = new NebimExpenseModel
                    {
                        ExpenseCode = reader["ExpenseCode"]?.ToString(),
                        ExpenseDescription = reader["ExpenseDescription"]?.ToString(),
                        ItemTaxGrCode = reader["ItemTaxGrCode"]?.ToString(),
                        ItemTaxGrDescription = reader["ItemTaxGrDescription"]?.ToString(),
                        IsBlocked = reader["IsBlocked"] != DBNull.Value && Convert.ToBoolean(reader["IsBlocked"])
                    };

                    nebimExpenseModels.Add(invoice);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return nebimExpenseModels.ToList();
        }

        //Tedarikçiler
        //public List<IncomingEInvoiceLineModel> GetVendorList(int companyId)
        //{
        //    List<IncomingEInvoiceLineModel> eInvoiceHeaderModels = new List<IncomingEInvoiceLineModel>();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(companyId == 1 ? connectionStringDEM : connectionStringKEP))
        //        {
        //            using (SqlCommand command = new SqlCommand("usp_Entegrasyon_VendorList", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;

        //                connection.Open();

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var invoice = new IncomingEInvoiceLineModel
        //                        {
        //                            // ===== Invoice Header =====
        //                            InvoiceHeaderID = reader.GetGuid(reader.GetOrdinal("InvoiceHeaderID")),
        //                            EInvoiceNumber = reader["EInvoiceNumber"]?.ToString(),
        //                            ProcessCode = reader["ProcessCode"]?.ToString(),
        //                            IsReturn = reader["IsReturn"] != DBNull.Value && Convert.ToBoolean(reader["IsReturn"]),
        //                            InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
        //                        };

        //                        eInvoiceHeaderModels.Add(invoice);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Hata: " + ex.Message);
        //    }
        //    return eInvoiceHeaderModels.OrderBy(x => x.SortOrder).ToList();
        //}

        public List<IncomingEInvoiceHeaderModel> GetIncomingEInvoiceHeaderModels(Guid? uuid = null)
        {
            List<IncomingEInvoiceHeaderModel> eInvoiceHeaderModels = new List<IncomingEInvoiceHeaderModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringDEM))
                {
                    using (SqlCommand command = new SqlCommand("usp_AcceptIncomingEInvoiceHeaders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@StartDate", new DateTime(2026, 2, 1)));
                        command.Parameters.Add(new SqlParameter("@EndDate", DateTime.Now.AddDays(1)));
                        if(uuid != null)
                            command.Parameters.Add(new SqlParameter("@UUID", uuid));

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var invoice = new IncomingEInvoiceHeaderModel
                                {
                                    CompanyName = "DEM",
                                    InvoiceHeaderID = reader.GetGuid(reader.GetOrdinal("InvoiceHeaderID")),
                                    FromIntegratorUUID = reader["FromIntegrator_UUID"].ToString(),
                                    EInvoiceNumber = reader["EInvoiceNumber"].ToString(),
                                    ProcessCode = reader["ProcessCode"]?.ToString(),
                                    IsReturn = Convert.ToBoolean(reader["IsReturn"]),
                                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                    InvoiceTime = reader.GetDateTime(reader.GetOrdinal("InvoiceTime")),
                                    Description = reader["Description"] as string,
                                    ConfirmationStatusCode = Convert.ToInt32(reader["ConfirmationStatusCode"]),
                                    CurrAccTypeCode = Convert.ToInt32(reader["CurrAccTypeCode"]),
                                    CurrAccCode = reader["CurrAccCode"].ToString(),
                                    PayableAmount = reader.GetDecimal(reader.GetOrdinal("PayableAmount")),
                                    CurrAccDescription = reader["CurrAccDescription"].ToString(),
                                    TaxNumber = reader["TaxNumber"].ToString(),
                                    IdentityNum = reader["IdentityNum"].ToString(),
                                    TaxExemptionDescription = reader["TaxExemptionDescription"].ToString(),
                                    StoppageRate = reader["StoppageRate"] == DBNull.Value ? null : (double?)reader["StoppageRate"],
                                    DocCurrencyCode = reader["DocCurrencyCode"].ToString(),
                                    TDisRate1 = reader["TDisRate1"] == DBNull.Value ? null : (double?)reader["TDisRate1"],
                                    TDisRate2 = reader["TDisRate2"] == DBNull.Value ? null : (double?)reader["TDisRate2"],
                                    TDisRate3 = reader["TDisRate3"] == DBNull.Value ? null : (double?)reader["TDisRate3"],
                                    TDisRate4 = reader["TDisRate4"] == DBNull.Value ? null : (double?)reader["TDisRate4"],
                                    TDisRate5 = reader["TDisRate5"] == DBNull.Value ? null : (double?)reader["TDisRate5"],
                                    IsExpenseSlip = Convert.ToBoolean(reader["IsExpenseSlip"]),
                                    CompanyCode = reader["CompanyCode"].ToString(),
                                    OfficeCode = reader["OfficeCode"].ToString(),
                                    EInvoiceAliasCode = reader["EInvoiceAliasCode"].ToString()
                                };

                                eInvoiceHeaderModels.Add(invoice);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionStringKEP))
                {
                    using (SqlCommand command = new SqlCommand("usp_AcceptIncomingEInvoiceHeaders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@StartDate", new DateTime(2026, 2, 1)));
                        command.Parameters.Add(new SqlParameter("@EndDate", DateTime.Now.AddDays(1)));
                        if (uuid != null)
                            command.Parameters.Add(new SqlParameter("@UUID", uuid));

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var invoice = new IncomingEInvoiceHeaderModel
                                {
                                    CompanyName = "KEP",
                                    InvoiceHeaderID = reader.GetGuid(reader.GetOrdinal("InvoiceHeaderID")),
                                    FromIntegratorUUID = reader["FromIntegrator_UUID"].ToString(),
                                    EInvoiceNumber = reader["EInvoiceNumber"].ToString(),
                                    ProcessCode = reader["ProcessCode"]?.ToString(),
                                    IsReturn = Convert.ToBoolean(reader["IsReturn"]),
                                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                    InvoiceTime = reader.GetDateTime(reader.GetOrdinal("InvoiceTime")),
                                    Description = reader["Description"] as string,
                                    ConfirmationStatusCode = Convert.ToInt32(reader["ConfirmationStatusCode"]),
                                    CurrAccTypeCode = Convert.ToInt32(reader["CurrAccTypeCode"]),
                                    CurrAccCode = reader["CurrAccCode"].ToString(),
                                    PayableAmount = reader.GetDecimal(reader.GetOrdinal("PayableAmount")),
                                    CurrAccDescription = reader["CurrAccDescription"].ToString(),
                                    TaxNumber = reader["TaxNumber"].ToString(),
                                    IdentityNum = reader["IdentityNum"].ToString(),
                                    TaxExemptionDescription = reader["TaxExemptionDescription"].ToString(),
                                    StoppageRate = reader["StoppageRate"] == DBNull.Value ? null : (double?)reader["StoppageRate"],
                                    DocCurrencyCode = reader["DocCurrencyCode"].ToString(),
                                    TDisRate1 = reader["TDisRate1"] == DBNull.Value ? null : (double?)reader["TDisRate1"],
                                    TDisRate2 = reader["TDisRate2"] == DBNull.Value ? null : (double?)reader["TDisRate2"],
                                    TDisRate3 = reader["TDisRate3"] == DBNull.Value ? null : (double?)reader["TDisRate3"],
                                    TDisRate4 = reader["TDisRate4"] == DBNull.Value ? null : (double?)reader["TDisRate4"],
                                    TDisRate5 = reader["TDisRate5"] == DBNull.Value ? null : (double?)reader["TDisRate5"],
                                    IsExpenseSlip = Convert.ToBoolean(reader["IsExpenseSlip"]),
                                    CompanyCode = reader["CompanyCode"].ToString(),
                                    OfficeCode = reader["OfficeCode"].ToString(),
                                    EInvoiceAliasCode = reader["EInvoiceAliasCode"].ToString()
                                };

                                eInvoiceHeaderModels.Add(invoice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return eInvoiceHeaderModels.OrderByDescending(x=>x.InvoiceDate).ToList();
        }

        public List<IncomingEInvoiceLineModel> GetIncomingEInvoiceLineModels(Guid uuid)
        {
            List<IncomingEInvoiceLineModel> eInvoiceHeaderModels = new List<IncomingEInvoiceLineModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringDEM))
                {
                    using (SqlCommand command = new SqlCommand("usp_AcceptIncomingEInvoices", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@UUID", uuid));

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var invoice = new IncomingEInvoiceLineModel
                                {
                                    // ===== Invoice Header =====
                                    InvoiceHeaderID = reader.GetGuid(reader.GetOrdinal("InvoiceHeaderID")),
                                    EInvoiceNumber = reader["EInvoiceNumber"]?.ToString(),
                                    ProcessCode = reader["ProcessCode"]?.ToString(),
                                    IsReturn = reader["IsReturn"] != DBNull.Value && Convert.ToBoolean(reader["IsReturn"]),
                                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                    InvoiceTime = reader.GetDateTime(reader.GetOrdinal("InvoiceTime")),
                                    Description = reader["Description"]?.ToString(),
                                    ConfirmationStatusCode = reader["ConfirmationStatusCode"] != DBNull.Value ? Convert.ToInt32(reader["ConfirmationStatusCode"]) : 0,
                                    CurrAccTypeCode = reader["CurrAccTypeCode"] != DBNull.Value ? Convert.ToInt32(reader["CurrAccTypeCode"]) : 0,
                                    PayableAmount = reader.GetDecimal(reader.GetOrdinal("PayableAmount")),
                                    CurrAccCode = reader["CurrAccCode"]?.ToString(),
                                    CurrAccDescription = reader["CurrAccDescription"]?.ToString(),
                                    TaxNumber = reader["TaxNumber"]?.ToString(),
                                    IdentityNum = reader["IdentityNum"]?.ToString(),
                                    TaxTypeCode = reader["TaxTypeCode"] != DBNull.Value ? Convert.ToInt32(reader["TaxTypeCode"]) : 0,
                                    WithHoldingTaxTypeCode = reader["WithHoldingTaxTypeCode"]?.ToString(),
                                    DovCode = reader["DovCode"]?.ToString(),
                                    TaxExemptionDescription = reader["TaxExemptionDescription"]?.ToString(),
                                    StoppageRate = reader["StoppageRate"] != DBNull.Value ? Convert.ToDouble(reader["StoppageRate"]) : 0,
                                    DocCurrencyCode = reader["DocCurrencyCode"]?.ToString(),
                                    ExchangeRate = reader["ExchangeRate"] != DBNull.Value ? Convert.ToDecimal(reader["ExchangeRate"]) : 0,
                                    TDisRate1 = reader["TDisRate1"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate1"]) : 0,
                                    TDisRate2 = reader["TDisRate2"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate2"]) : 0,
                                    TDisRate3 = reader["TDisRate3"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate3"]) : 0,
                                    TDisRate4 = reader["TDisRate4"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate4"]) : 0,
                                    TDisRate5 = reader["TDisRate5"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate5"]) : 0,
                                    IsExpenseSlip = reader["IsExpenseSlip"] != DBNull.Value && Convert.ToBoolean(reader["IsExpenseSlip"]),
                                    CompanyCode = reader["CompanyCode"]?.ToString(),
                                    OfficeCode = reader["OfficeCode"]?.ToString(),
                                    EInvoiceAliasCode = reader["EInvoiceAliasCode"]?.ToString(),

                                    // ===== Invoice Line =====
                                    SortOrder = reader["SortOrder"] != DBNull.Value ? Convert.ToInt32(reader["SortOrder"]) : 0,
                                    ItemTypeCode = reader["ItemTypeCode"] != DBNull.Value ? Convert.ToInt32(reader["ItemTypeCode"]) : 0,
                                    ItemCode = reader["ItemCode"]?.ToString(),
                                    ColorCode = reader["ColorCode"]?.ToString(),
                                    ItemDim1Code = reader["ItemDim1Code"]?.ToString(),
                                    ItemDim2Code = reader["ItemDim2Code"]?.ToString(),
                                    ItemDim3Code = reader["ItemDim3Code"]?.ToString(),
                                    ItemName = reader["ItemName"]?.ToString(),
                                    UnitOfMeasureCode = reader["UnitOfMeasureCode"]?.ToString(),
                                    Qty1 = reader["Qty1"] != DBNull.Value ? Convert.ToDecimal(reader["Qty1"]) : 0,
                                    UsedBarcode = reader["UsedBarcode"]?.ToString(),
                                    VatRate = reader["VatRate"] != DBNull.Value ? Convert.ToDecimal(reader["VatRate"]) : 0,
                                    PCTCode = reader["PCTCode"]?.ToString(),
                                    PCTRate = reader["PCTRate"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PCTRate"]) : null,
                                    LDisRate1 = reader["LDisRate1"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate1"]) : null,
                                    LDisRate2 = reader["LDisRate2"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate2"]) : null,
                                    LDisRate3 = reader["LDisRate3"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate3"]) : null,
                                    LDisRate4 = reader["LDisRate4"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate4"]) : null,
                                    LDisRate5 = reader["LDisRate5"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate5"]) : null,
                                    PriceCurrencyCode = reader["PriceCurrencyCode"]?.ToString(),
                                    PriceExchangeRate = reader["PriceExchangeRate"] != DBNull.Value ? Convert.ToDecimal(reader["PriceExchangeRate"]) : 0,
                                    LineDescription = reader["LineDescription"]?.ToString(),
                                    ManufacturersItemIdentification = reader["ManufacturersItemIdentification"]?.ToString(),

                                    // ===== Invoice Line Currency =====
                                    Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                                    Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                    LDiscount1 = reader["LDiscount1"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount1"]) : 0,
                                    LDiscount2 = reader["LDiscount2"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount2"]) : 0,
                                    LDiscount3 = reader["LDiscount3"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount3"]) : 0,
                                    LDiscount4 = reader["LDiscount4"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount4"]) : 0,
                                    LDiscount5 = reader["LDiscount5"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount5"]) : 0,
                                    TDiscount1 = reader["TDiscount1"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount1"]) : 0,
                                    TDiscount2 = reader["TDiscount2"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount2"]) : 0,
                                    TDiscount3 = reader["TDiscount3"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount3"]) : 0,
                                    TDiscount4 = reader["TDiscount4"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount4"]) : 0,
                                    TDiscount5 = reader["TDiscount5"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount5"]) : 0,
                                    TaxBase = reader["TaxBase"] != DBNull.Value ? Convert.ToDecimal(reader["TaxBase"]) : 0,
                                    Pct = reader["Pct"] != DBNull.Value ? Convert.ToDecimal(reader["Pct"]) : 0,
                                    Vat = reader["Vat"] != DBNull.Value ? Convert.ToDecimal(reader["Vat"]) : 0,
                                    NetAmount = reader["NetAmount"] != DBNull.Value ? Convert.ToDecimal(reader["NetAmount"]) : 0,

                                    // ===== Expense Slip =====
                                    Expense_SortOrder = reader["Expense_SortOrder"] != DBNull.Value ? (int?)Convert.ToInt32(reader["Expense_SortOrder"]) : null,
                                    Expense_GLAccCode = reader["Expense_GLAccCode"]?.ToString(),
                                    Expense_LineDescription = reader["Expense_LineDescription"]?.ToString(),
                                    Expense_ItemDescription = reader["Expense_ItemDescription"]?.ToString(),
                                    Expense_TaxRate = reader["Expense_TaxRate"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_TaxRate"]) : null,
                                    Expense_Tax = reader["Expense_Tax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_Tax"]) : null,
                                    Expense_Amount = reader["Expense_Amount"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_Amount"]) : null,
                                    Expense_TaxAmount = reader["Expense_TaxAmount"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_TaxAmount"]) : null,
                                    Expense_TaxAssessment = reader["Expense_TaxAssessment"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_TaxAssessment"]) : null,
                                    Expense_PriceCurrencyCode = reader["Expense_PriceCurrencyCode"]?.ToString(),
                                    Expense_PriceExchangeRate = reader["Expense_PriceExchangeRate"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_PriceExchangeRate"]) : null,

                                    // ===== Postal Address =====
                                    TaxOfficeName = reader["TaxOfficeName"]?.ToString(),
                                    TaxOfficeCode = reader["TaxOfficeCode"]?.ToString(),
                                    FirstName = reader["FirstName"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    StreetName = reader["StreetName"]?.ToString(),
                                    BuildingNumber = reader["BuildingNumber"]?.ToString(),
                                    CitySubdivisionName = reader["CitySubdivisionName"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    CityCode = reader["CityCode"]?.ToString(),
                                    PostalZone = reader["PostalZone"]?.ToString(),
                                    CountryName = reader["CountryName"]?.ToString(),
                                    CountryCode = reader["CountryCode"]?.ToString(),

                                    // ===== Additional Info =====
                                    OrderInfo = reader["OrderInfo"]?.ToString(),
                                    ShipmentInfo = reader["ShipmentInfo"]?.ToString(),
                                    CostCenterCode = reader["CostCenterCode"]?.ToString()
                                };

                                eInvoiceHeaderModels.Add(invoice);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionStringKEP))
                {
                    using (SqlCommand command = new SqlCommand("usp_AcceptIncomingEInvoices", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@UUID", uuid));

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var invoice = new IncomingEInvoiceLineModel
                                {
                                    // ===== Invoice Header =====
                                    InvoiceHeaderID = reader.GetGuid(reader.GetOrdinal("InvoiceHeaderID")),
                                    EInvoiceNumber = reader["EInvoiceNumber"]?.ToString(),
                                    ProcessCode = reader["ProcessCode"]?.ToString(),
                                    IsReturn = reader["IsReturn"] != DBNull.Value && Convert.ToBoolean(reader["IsReturn"]),
                                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate")),
                                    InvoiceTime = reader.GetDateTime(reader.GetOrdinal("InvoiceTime")),
                                    Description = reader["Description"]?.ToString(),
                                    ConfirmationStatusCode = reader["ConfirmationStatusCode"] != DBNull.Value ? Convert.ToInt32(reader["ConfirmationStatusCode"]) : 0,
                                    CurrAccTypeCode = reader["CurrAccTypeCode"] != DBNull.Value ? Convert.ToInt32(reader["CurrAccTypeCode"]) : 0,
                                    PayableAmount = reader.GetDecimal(reader.GetOrdinal("PayableAmount")),
                                    CurrAccCode = reader["CurrAccCode"]?.ToString(),
                                    CurrAccDescription = reader["CurrAccDescription"]?.ToString(),
                                    TaxNumber = reader["TaxNumber"]?.ToString(),
                                    IdentityNum = reader["IdentityNum"]?.ToString(),
                                    TaxTypeCode = reader["TaxTypeCode"] != DBNull.Value ? Convert.ToInt32(reader["TaxTypeCode"]) : 0,
                                    WithHoldingTaxTypeCode = reader["WithHoldingTaxTypeCode"]?.ToString(),
                                    DovCode = reader["DovCode"]?.ToString(),
                                    TaxExemptionDescription = reader["TaxExemptionDescription"]?.ToString(),
                                    StoppageRate = reader["StoppageRate"] != DBNull.Value ? Convert.ToDouble(reader["StoppageRate"]) : 0,
                                    DocCurrencyCode = reader["DocCurrencyCode"]?.ToString(),
                                    ExchangeRate = reader["ExchangeRate"] != DBNull.Value ? Convert.ToDecimal(reader["ExchangeRate"]) : 0,
                                    TDisRate1 = reader["TDisRate1"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate1"]) : 0,
                                    TDisRate2 = reader["TDisRate2"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate2"]) : 0,
                                    TDisRate3 = reader["TDisRate3"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate3"]) : 0,
                                    TDisRate4 = reader["TDisRate4"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate4"]) : 0,
                                    TDisRate5 = reader["TDisRate5"] != DBNull.Value ? Convert.ToDouble(reader["TDisRate5"]) : 0,
                                    IsExpenseSlip = reader["IsExpenseSlip"] != DBNull.Value && Convert.ToBoolean(reader["IsExpenseSlip"]),
                                    CompanyCode = reader["CompanyCode"]?.ToString(),
                                    OfficeCode = reader["OfficeCode"]?.ToString(),
                                    EInvoiceAliasCode = reader["EInvoiceAliasCode"]?.ToString(),

                                    // ===== Invoice Line =====
                                    SortOrder = reader["SortOrder"] != DBNull.Value ? Convert.ToInt32(reader["SortOrder"]) : 0,
                                    ItemTypeCode = reader["ItemTypeCode"] != DBNull.Value ? Convert.ToInt32(reader["ItemTypeCode"]) : 0,
                                    ItemCode = reader["ItemCode"]?.ToString(),
                                    ColorCode = reader["ColorCode"]?.ToString(),
                                    ItemDim1Code = reader["ItemDim1Code"]?.ToString(),
                                    ItemDim2Code = reader["ItemDim2Code"]?.ToString(),
                                    ItemDim3Code = reader["ItemDim3Code"]?.ToString(),
                                    ItemName = reader["ItemName"]?.ToString(),
                                    UnitOfMeasureCode = reader["UnitOfMeasureCode"]?.ToString(),
                                    Qty1 = reader["Qty1"] != DBNull.Value ? Convert.ToDecimal(reader["Qty1"]) : 0,
                                    UsedBarcode = reader["UsedBarcode"]?.ToString(),
                                    VatRate = reader["VatRate"] != DBNull.Value ? Convert.ToDecimal(reader["VatRate"]) : 0,
                                    PCTCode = reader["PCTCode"]?.ToString(),
                                    PCTRate = reader["PCTRate"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PCTRate"]) : null,
                                    LDisRate1 = reader["LDisRate1"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate1"]) : null,
                                    LDisRate2 = reader["LDisRate2"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate2"]) : null,
                                    LDisRate3 = reader["LDisRate3"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate3"]) : null,
                                    LDisRate4 = reader["LDisRate4"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate4"]) : null,
                                    LDisRate5 = reader["LDisRate5"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["LDisRate5"]) : null,
                                    PriceCurrencyCode = reader["PriceCurrencyCode"]?.ToString(),
                                    PriceExchangeRate = reader["PriceExchangeRate"] != DBNull.Value ? Convert.ToDecimal(reader["PriceExchangeRate"]) : 0,
                                    LineDescription = reader["LineDescription"]?.ToString(),
                                    ManufacturersItemIdentification = reader["ManufacturersItemIdentification"]?.ToString(),

                                    // ===== Invoice Line Currency =====
                                    Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                                    Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                    LDiscount1 = reader["LDiscount1"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount1"]) : 0,
                                    LDiscount2 = reader["LDiscount2"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount2"]) : 0,
                                    LDiscount3 = reader["LDiscount3"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount3"]) : 0,
                                    LDiscount4 = reader["LDiscount4"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount4"]) : 0,
                                    LDiscount5 = reader["LDiscount5"] != DBNull.Value ? Convert.ToDecimal(reader["LDiscount5"]) : 0,
                                    TDiscount1 = reader["TDiscount1"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount1"]) : 0,
                                    TDiscount2 = reader["TDiscount2"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount2"]) : 0,
                                    TDiscount3 = reader["TDiscount3"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount3"]) : 0,
                                    TDiscount4 = reader["TDiscount4"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount4"]) : 0,
                                    TDiscount5 = reader["TDiscount5"] != DBNull.Value ? Convert.ToDecimal(reader["TDiscount5"]) : 0,
                                    TaxBase = reader["TaxBase"] != DBNull.Value ? Convert.ToDecimal(reader["TaxBase"]) : 0,
                                    Pct = reader["Pct"] != DBNull.Value ? Convert.ToDecimal(reader["Pct"]) : 0,
                                    Vat = reader["Vat"] != DBNull.Value ? Convert.ToDecimal(reader["Vat"]) : 0,
                                    NetAmount = reader["NetAmount"] != DBNull.Value ? Convert.ToDecimal(reader["NetAmount"]) : 0,

                                    // ===== Expense Slip =====
                                    Expense_SortOrder = reader["Expense_SortOrder"] != DBNull.Value ? (int?)Convert.ToInt32(reader["Expense_SortOrder"]) : null,
                                    Expense_GLAccCode = reader["Expense_GLAccCode"]?.ToString(),
                                    Expense_LineDescription = reader["Expense_LineDescription"]?.ToString(),
                                    Expense_ItemDescription = reader["Expense_ItemDescription"]?.ToString(),
                                    Expense_TaxRate = reader["Expense_TaxRate"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_TaxRate"]) : null,
                                    Expense_Tax = reader["Expense_Tax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_Tax"]) : null,
                                    Expense_Amount = reader["Expense_Amount"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_Amount"]) : null,
                                    Expense_TaxAmount = reader["Expense_TaxAmount"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_TaxAmount"]) : null,
                                    Expense_TaxAssessment = reader["Expense_TaxAssessment"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_TaxAssessment"]) : null,
                                    Expense_PriceCurrencyCode = reader["Expense_PriceCurrencyCode"]?.ToString(),
                                    Expense_PriceExchangeRate = reader["Expense_PriceExchangeRate"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Expense_PriceExchangeRate"]) : null,

                                    // ===== Postal Address =====
                                    TaxOfficeName = reader["TaxOfficeName"]?.ToString(),
                                    TaxOfficeCode = reader["TaxOfficeCode"]?.ToString(),
                                    FirstName = reader["FirstName"]?.ToString(),
                                    LastName = reader["LastName"]?.ToString(),
                                    StreetName = reader["StreetName"]?.ToString(),
                                    BuildingNumber = reader["BuildingNumber"]?.ToString(),
                                    CitySubdivisionName = reader["CitySubdivisionName"]?.ToString(),
                                    CityName = reader["CityName"]?.ToString(),
                                    CityCode = reader["CityCode"]?.ToString(),
                                    PostalZone = reader["PostalZone"]?.ToString(),
                                    CountryName = reader["CountryName"]?.ToString(),
                                    CountryCode = reader["CountryCode"]?.ToString(),

                                    // ===== Additional Info =====
                                    OrderInfo = reader["OrderInfo"]?.ToString(),
                                    ShipmentInfo = reader["ShipmentInfo"]?.ToString(),
                                    CostCenterCode = reader["CostCenterCode"]?.ToString()
                                };

                                eInvoiceHeaderModels.Add(invoice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
            return eInvoiceHeaderModels.OrderBy(x => x.SortOrder).ToList();
        }
    }
}
