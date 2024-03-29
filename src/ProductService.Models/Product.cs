﻿namespace ProductService.Models;

public class Product
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public double Price { get; private set; }

    public bool Active { get; private set; }

    public Product(string name, string description, double price, bool active)
    {
        Name = name;
        Description = description;
        Price = price;
        Active = active;
    }
}
