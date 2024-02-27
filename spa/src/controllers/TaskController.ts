import { IBaseDTO, ITaskElementsCollection, TasksService } from "../model/TasksService"

export class TaskController {
  static createTaskBase = async(baseElementDTO: IBaseDTO) => {
    return await TasksService.createTaskBase(baseElementDTO);
  }

  static updateTasks = async(taskElements: ITaskElementsCollection) => {
    return await TasksService.updateTasks(taskElements);
  }

  static getTasks = async() => {
    return await TasksService.getTasks();
  }

  static deleteTask = async(bid: string) => {
    return await TasksService.deleteTask(bid);
  }
}
