﻿
namespace Sufra.DTOs.SufraEmpDTOs
{
    public class AdminLoginResponseDTO
    {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
    }
}
