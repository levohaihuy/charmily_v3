﻿using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryDAO _categoryDAO;

    public CategoryRepository(CategoryDAO categoryDAO)
    {
        _categoryDAO = categoryDAO;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _categoryDAO.GetAllCategoriesAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(long categoryId)
    {
        return await _categoryDAO.GetCategoryByIdAsync(categoryId);
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
        return await _categoryDAO.GetCategoryByNameAsync(name);
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        return await _categoryDAO.UpdateCategoryAsync(category);
    }

    public async Task DeleteCategoryAsync(long categoryId)
    {
        await _categoryDAO.DeleteCategoryAsync(categoryId);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        return await _categoryDAO.CreateCategoryAsync(category);
    }
}