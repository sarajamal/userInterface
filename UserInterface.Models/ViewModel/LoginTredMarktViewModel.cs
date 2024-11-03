using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;
using Test12.Models.Models;
using Test12.Models.Models.Clean;
using Test12.Models.Models.Device_Tools;
using Test12.Models.Models.Food;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.Production;
using Test12.Models.Models.ReadyFood;
using Test12.Models.Models.trade_mark;

namespace Test12.Models.ViewModel
{
    public class LoginTredMarktViewModel
    {
        [ValidateNever]
        [JsonIgnore]
        public List<Brands> tredList { get; set; }

        [ValidateNever]
        public Brands TredMarktVM { get; set; }
        [ValidateNever]
        public PreparationIngredients ComponentVM { get; set; }
        [ValidateNever]
        public List<Preparations> PreparatonLoginVMlist { get; set; }

        [ValidateNever]
        public List<Production> ProductionLoginVMlist { get; set; }

        //-------------Food -----------------------------------------
        [ValidateNever]
        [JsonIgnore]
        public List<FoodStuffs> FoodLoginVMlist { get; set; }
        [ValidateNever]
        public FoodStuffs FoodLoginVM { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<FoodStuffs> FoodsLoginVMorder { get; set; }
        [ValidateNever]
        public Brands tredMaeketFoodsVM { get; set; }
        [ValidateNever]
        public LoginTredMarktViewModel WelcomTredmarketFood { get; set; }
        //===============================================================//

        //--------------------Device And tools --------------------------
        [ValidateNever]
        public List<DevicesAndTools> DeviceToolsLoginVMlist { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<DevicesAndTools> Devices_toolsVMorder { get; set; }
        [ValidateNever]
        public Brands tredMaeketToolsVM { get; set; }
        [ValidateNever]
        public DevicesAndTools DeviceToolsLoginVM { get; set; }
        [ValidateNever]
        public LoginTredMarktViewModel WelcomtredmarketDeviceTools { get; set; }
        //=================================================================//

        //--------------------ReadyFood -------------------------------------

        [ValidateNever]
        [JsonIgnore]
        public List<ReadyProducts> ReadyFoodLoginVMlist { get; set; }

        [ValidateNever]
        public ReadyProducts ReadyFoodLoginVM { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<ReadyProducts> FoodReadyVMorder { get; set; }
        [ValidateNever]
        public Brands tredMaeketReadyfoodVM { get; set; }
        [ValidateNever]
        public LoginTredMarktViewModel WelcomTredmarketReadyFood { get; set; }
        //=================================================================//

        //--------------------Preparation -------------------------------------
        [ValidateNever]
        public Preparations PreparationVM { get; set; }
        [ValidateNever]
        public List<PreparationIngredients> componontVMList { get; set; }
        [ValidateNever]
        public PreparationIngredients componontVM { get; set; }
        [ValidateNever]
        public PreparationTools ToolsVarityVM { get; set; }
        [ValidateNever]
        public List<PreparationTools> PreparationsTools { get; set; }
        [ValidateNever]
        public List<PreparationTools> ToolsVarityVMList { get; set; }
        [ValidateNever]
        public List<PreparationSteps> stepsVM { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<Brands> tredMaeketVMList { get; set; }
        [ValidateNever]
        public LoginTredMarktViewModel WelcomTredMarketPrecomponent { set; get; }
        [ValidateNever]
        public Brands tredMaeketVM { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<Preparations> PreparationList { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public List<Preparations> PreparationListCount { get; set; }
        //===============================================================================

        //--------------------Production -------------------------------------
        [ValidateNever]
        public LoginTredMarktViewModel welcomTredmarketProduction { set; get; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<Production> itemsList { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<Production> itemList33333 { get; set; }
        [ValidateNever]
        public Production Productionvm { get; set; }

        [ValidateNever]
        public List<ProductionIngredients> componontVMList2 { get; set; }
        [ValidateNever]
        public ProductionIngredients componontVM2 { get; set; }
        [ValidateNever]
        public List<ProductionTools> ToolsVarityVM2List { get; set; }
        [ValidateNever]
        public ProductionTools ToolsVarityVM2 { get; set; }
        [ValidateNever]
        public List<ProductionSteps> stepsVM2List { get; set; }
        [ValidateNever]
        public ProductionSteps stepsVM2 { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public List<Production> ProductionListCount { get; set; }
        //====================================================================

        //-------------------- Cleaning  -------------------------------------
        [ValidateNever]
        public List<Cleaning> CleanLoginVMlist { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public List<Cleaning> CleanList { get; set; }
        [ValidateNever]
        public List<CleaningSteps> CleaningStepsList { get; set; }

        [ValidateNever]
        public Brands tredMaeketCleanVM { get; set; }
        [ValidateNever]
        public LoginTredMarktViewModel WelcomTredMarketClean { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<Cleaning> CleaningVMorder { get; set; }
        [ValidateNever]
        public Cleaning CleanViewModel { get; set; }
        //=======================================================================

        [ValidateNever]
        public List<MainSections> MainsectionVMlist { get; set; }

        [ValidateNever]
        public MainSections MainsectionVM { get; set; }

        [ValidateNever]
        public int PageNumber { get; set; }







    }
}
