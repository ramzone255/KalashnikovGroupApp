﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KalashnikovGroupApp.Models;

namespace KalashnikovGroupApp.Servises
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7037/") // Укажите правильный адрес вашего API
            };
        }

        internal async Task<List<Employees>> GetEmployees()
        {
            var response = await _httpClient.GetAsync("api/Employees");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Employees>>();
        }
        internal async Task<List<Payday>> GetPaydayCollection()
        {
            var response = await _httpClient.GetAsync("api/Payday");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Payday>>();
        }
        internal async Task<List<Components>> GetComponents()
        {
            var response = await _httpClient.GetAsync("api/Components");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Components>>();
        }
        internal async Task<List<Deal>> GetDeal()
        {
            var response = await _httpClient.GetAsync("api/Deal");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Deal>>();
        }
        internal async Task CreateEmployees(Employees employees)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Employees/POST?id_post={employees.Postid_post}", employees);
            response.EnsureSuccessStatusCode();
        }
        internal async Task CreatePayday(Payday payday)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Payday/POST", payday);
            response.EnsureSuccessStatusCode();
        }
        internal async Task CreateComponents(Components сomponents)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Components/POST", сomponents);
            response.EnsureSuccessStatusCode();
        }
        internal async Task CreateDeal(Deal deal)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Deal/POST", deal);
            response.EnsureSuccessStatusCode();
        }

        internal async Task UpdateEmployees(Employees employees)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Employees/PUT/{employees.id_employess}", employees);
            response.EnsureSuccessStatusCode();
        }
        internal async Task UpdatePayday(Payday payday)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Payday/PUT/{payday.id_payday}", payday);
            response.EnsureSuccessStatusCode();
        }
        internal async Task UpdateComponents(Components сomponents)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Components/PUT/{сomponents.id_components}", сomponents);
            response.EnsureSuccessStatusCode();
        }
        internal async Task UpdateDeal(Deal deal)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Deal/PUT/{deal.id_deal}", deal);
            response.EnsureSuccessStatusCode();
        }

        internal async Task DeleteEmployees(int id_employees)
        {
            var response = await _httpClient.DeleteAsync($"api/Employees/DELETE/{id_employees}");
            response.EnsureSuccessStatusCode();
        }
        internal async Task DeletePayday(int id_payday)
        {
            var response = await _httpClient.DeleteAsync($"api/Payday/DELETE/{id_payday}");
            response.EnsureSuccessStatusCode();
        }
        internal async Task DeleteСomponents(int id_components)
        {
            var response = await _httpClient.DeleteAsync($"api/Components/DELETE/{id_components}");
            response.EnsureSuccessStatusCode();
        }
        internal async Task DeleteDeal(int id_deal)
        {
            var response = await _httpClient.DeleteAsync($"api/Deal/DELETE/{id_deal}");
            response.EnsureSuccessStatusCode();
        }
        internal async Task<Employees> AuthenticateAsync(string mail, string password)
        {
            var login = new Login { mail = mail, password = password };
            var response = await _httpClient.PostAsJsonAsync("api/Employees/SingIn", login);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Employees>();
            }
            return null;
        }

        // Другие методы для взаимодействия с API
    }
}
