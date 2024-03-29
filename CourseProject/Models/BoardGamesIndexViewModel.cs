﻿namespace CourseProject.Models;

public class BoardGamesIndexViewModel
{
    public List<BoardGameViewModel> BoardGames { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalGames { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}