using System;
using System.Collections.Generic;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Interfaces;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Implementations {

    public class Repository<T> : IRepository<T> where T : class {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TestingEnvironment testingSettings;
        protected readonly AppDbContext context;
        private readonly UserManager<UserExtended> userManager;

        public Repository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IOptions<TestingEnvironment> testingSettings, UserManager<UserExtended> userManager) {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.testingSettings = testingSettings.Value;
            this.userManager = userManager;
        }

        public T Create(T entity) {
            using var transaction = context.Database.BeginTransaction();
            context.Add(entity);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return entity;
        }

        public void CreateList(List<T> entities) {
            using var transaction = context.Database.BeginTransaction();
            context.AddRange(entities);
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public T Update(T entity) {
            using var transaction = context.Database.BeginTransaction();
            context.Set<T>().Update(entity);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return entity;
        }

        public void Delete(T entity) {
            using var transaction = context.Database.BeginTransaction();
            try {
                context.Remove(entity);
                context.SaveChanges();
                DisposeOrCommit(transaction);
            }
            catch (Exception) {
                throw new CustomException {
                    ResponseCode = 491
                };
            }
        }

        public void DeleteRange(IEnumerable<T> entities) {
            context.RemoveRange(entities);
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingSettings.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        public IMetadata AttachMetadataToPostDto(IMetadata entity) {
            entity.PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
            entity.PostUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName;
            entity.PutAt = entity.PostAt;
            entity.PutUser = entity.PostUser;
            return entity;
        }

        public IMetadata AttachMetadataToPutDto(IMetadata existingEntity, IMetadata updatedEntity) {
            updatedEntity.PostAt = existingEntity.PostAt;
            updatedEntity.PostUser = existingEntity.PostUser;
            updatedEntity.PutAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
            updatedEntity.PutUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName;
            return updatedEntity;
        }

    }

}