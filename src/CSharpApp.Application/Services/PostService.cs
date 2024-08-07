﻿using CSharpApp.Core.Config;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CSharpApp.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpClientWrapper _clientWrapper;
        private readonly ILogger<PostService> _logger;
        private readonly PostSettings _postSettings;

        public PostService(ILogger<PostService> logger, PostSettings postSettings,
            IHttpClientWrapper clientWrapper)
        {
            _logger = logger;
            _postSettings = postSettings;
            _clientWrapper = clientWrapper;
        }

        public async Task<PostRecord?> AddPost(PostRecord post)
        {
            try
            {
                return await _clientWrapper.Post(_postSettings.ItemRootUrl, post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create a new Post");
            }
            return new PostRecord(0, 0, "", "");
        }

        public async Task<PostRecord?> DeletePostById(int id)
        {
            try
            {
                var res = await _clientWrapper.Delete<PostRecord>(_postSettings.ItemRootUrl, id);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete Post with id {Id}",id);
            }
            return new PostRecord(0, 0, "", "");
        }

        public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts()
        {
            try
            {
                var response = await _clientWrapper.Get<List<PostRecord>>(_postSettings.ItemRootUrl);

                return response!.AsReadOnly();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get All Posts");
            }
            return new List<PostRecord>().AsReadOnly();
        }

        public async Task<PostRecord?> GetPostById(int id)
        {
            try
            {
                return await _clientWrapper.Get<PostRecord>(_postSettings.ItemRootUrl, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get Post by id {Id}",id);
            }
            return new PostRecord(0, 0, "", "");
        }
    }
}