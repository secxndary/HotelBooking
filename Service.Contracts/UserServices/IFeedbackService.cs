﻿using Entities.Models;
using Shared.DataTransferObjects.InputDtos;
using Shared.DataTransferObjects.OutputDtos;
using Shared.DataTransferObjects.UpdateDtos;
namespace Service.Contracts.UserServices;

public interface IFeedbackService
{
    IEnumerable<FeedbackDto> GetFeedbacksForHotel(Guid hotelId, bool trackChanges);
    IEnumerable<FeedbackDto> GetFeedbacksForRoom(Guid roomId, bool trackChanges);
    IEnumerable<FeedbackDto> GetFeedbacksForReservation(Guid reservationId, bool trackChanges);
    FeedbackDto GetFeedback(Guid id, bool trackChanges);
    FeedbackDto CreateFeedbackForReservation(Guid reservationId, 
        FeedbackForCreationDto feedback, bool trackChanges);
    void UpdateFeedback(Guid id, FeedbackForUpdateDto feedbackForUpdate, bool trackChanges);
    (FeedbackForUpdateDto feedbackToPatch, Feedback feedbackEntity) GetFeedbackForPatch
        (Guid id, bool trackChanges);
    void SaveChangesForPatch(FeedbackForUpdateDto feedbackToPatch, Feedback feedbackEntity);
    void DeleteFeedback(Guid id, bool trackChanges);
}