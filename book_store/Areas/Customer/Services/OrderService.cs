using Azure;
using book_store.Areas.Customer.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe.Climate;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using Stripe.Checkout;


namespace book_store.Areas.Customer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult AddOrderHeader(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork.OrderHeader.Add(orderHeader);
                _unitOfWork.Save();
                return ServiceResult.Ok("Added order header successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to add order header: {ex.Message}.");
            }
        }

        public ServiceResult<OrderHeader> GetOrderHeader(int id)
        {
            try
            {
                var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
                if (orderHeader == null) return ServiceResult<OrderHeader>.Fail("Id not found");

                return ServiceResult<OrderHeader>.Ok(orderHeader, "Success to get order header");
            }
            catch (Exception ex)
            {
                return ServiceResult<OrderHeader>.Fail($"Fail to retrive order header: {ex.Message}");
            }
        }

        public ServiceResult AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
                return ServiceResult.Ok("Added order detail successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to add order detail: {ex.Message}.");
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser", tracked:true);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.PaymentIntentId = paymentIntentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
            _unitOfWork.Save();
        }

        public void UpdateStatus(int id, string orderStatus, string paymentStatus)
        {
            var orderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser", tracked:true);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
            _unitOfWork.Save();
        }
    }
}
