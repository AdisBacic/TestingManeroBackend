﻿using Manero_Backend.Helpers.Factory;
using Manero_Backend.Models.Dtos.PromoCode;
using Manero_Backend.Models.Entities;
using Manero_Backend.Models.Interfaces.Repositories;
using Manero_Backend.Models.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manero_Backend.Helpers.Services
{
    public class PromoCodeService : BaseService<PromoCodeEntity>, IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IUserPromoCodeService _userPromoCodeService;
        public PromoCodeService(IPromoCodeRepository promoCodeRepository, IUserPromoCodeService userPromoCodeService) : base(promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _userPromoCodeService = userPromoCodeService;
        }

        public async Task<IActionResult> CreateAsync(PromoCodeEntity entity)
        {
            if (await _promoCodeRepository.CountAsync(x => x.Code == entity.Code) != 0)
                return HttpResultFactory.Conflict();

            await _promoCodeRepository.CreateAsync(entity);

            return HttpResultFactory.Created("", "");
        }

        public async Task<IActionResult> AddAsync(string code, string userId)
        {

            //Check if exists
            PromoCodeEntity promoCode = await _promoCodeRepository.GetAsync(x => x.Code == code);
            if(promoCode == null)
                return HttpResultFactory.NotFound("");

            //Check if expired
            if (promoCode.ValidToUnix < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                return HttpResultFactory.BadRequest(new { ErrorMessage = "Promocode has expired." });


            return await _userPromoCodeService.CreateAsync(new UserPromoCodeEntity() { AppUserId = userId, PromoCodeId = promoCode.Id });
        }

        public async Task<IActionResult> GetAllAsync(string userId)
        {
            return HttpResultFactory.Ok((await _promoCodeRepository.GetAllAsync(userId)).Select(x => (PromoCodeDto)x));
        }

        public async Task<IActionResult> GetValidateAsync(string code, string userId)
        {
            PromoCodeEntity promoCode = await _promoCodeRepository.GetAsync(x => x.Code == code);
            if (promoCode == null)
                return HttpResultFactory.NotFound("");

            //Check if expired
            if (promoCode.ValidToUnix < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                return HttpResultFactory.BadRequest(new { ErrorMessage = "Promocode has expired." });


            return HttpResultFactory.Ok((PromoCodeDto)await _promoCodeRepository.GetAsync(code));
            

            //return await _promoCodeRepository.GetAsync(x => x.UserPromoCodes.Where(y => y.AppUserId == userId && y.PromoCodeId == promoCode.Id && !y.Used).FirstOrDefault() != null) != null ? HttpResultFactory.Ok((PromoCodeDto)promoCode) : HttpResultFactory.BadRequest(new { ErrorMessage = "PromoCode already used."});
        }
    }
}
