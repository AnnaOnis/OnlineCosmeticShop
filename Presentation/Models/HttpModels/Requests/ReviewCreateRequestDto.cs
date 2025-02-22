﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Requests
{
    public class ReviewCreateRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Текст отзыва не должен превышать 1000 символов.")]
        public string ReviewText { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5.")]
        public int Rating { get; set; }
    }
}
