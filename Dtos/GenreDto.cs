namespace DevC_Core5_Api_Jwt.Dtos
{
    public class GenreDto
    {
        //kk=>Dto is general which viewModel is specific of =>a-transfering data from service to another or from layer to another  
        //b-encpsulation Model in case of sensetive properties

        //kk=>not write id as it auto generated // and client side validation for view model
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
