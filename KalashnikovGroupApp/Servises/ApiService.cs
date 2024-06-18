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

        internal async Task CreateEmployees(Employees employees)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Employees/POST?id_post={employees.Postid_post}", employees);
            response.EnsureSuccessStatusCode();
        }

        internal async Task UpdateEmployees(Employees employees)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Employees/PUT/{employees.id_employess}", employees);
            response.EnsureSuccessStatusCode();
        }

        internal async Task DeleteEmployees(int id_employees)
        {
            var response = await _httpClient.DeleteAsync($"api/Employees/DELETE/{id_employees}");
            response.EnsureSuccessStatusCode();
        }
        internal async Task<List<Post>> GetPost()
        {
            var response = await _httpClient.GetAsync("api/Post");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Post>>();
        }

        // Другие методы для взаимодействия с API
    }
}