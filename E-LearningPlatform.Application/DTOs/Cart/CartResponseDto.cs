using E_LearningPlatform.Application.DTOs.CartItem;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Cart
{
    public class CartResponseDto
    {
        public List<CartItemDto> Items { get; set; }
       = new();

        public decimal TotalPrice { get; set; }
    }
}
