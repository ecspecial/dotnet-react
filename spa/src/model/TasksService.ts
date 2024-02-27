import axios from 'axios';
import { AppConfig } from "./config"

export interface IBase {
  rid: string;
  bid: string;
  name: string;
  description: string;
}

// for POST
export interface IBaseDTO {
  rid: string;
  name: string;
  description: string;
}

export interface IPage {
  name: string;
  content: string;
}

export interface IJournal {
  id: string;
  rid: string;
  bid: string;
  pages: IPage[];
}

export interface IRelation {
  id: string;
  rid: string;
  bid: string;
  relative: string; // task bid
  relationType: number; // 0 - parent, 1 - child
}

export interface IChecklistItem {
  id: string;
  rid: string;
  bid: string;
  checked: boolean;
  resetOnComplete: boolean;
  parentID: string;
}

export interface ITimeLimit {
  id: string;
  rid: string;
  bid: string;
  deadline: string;
  executionTime: string;
  warningTime: string;
  repeatSpan: string;
  autoRepeat: boolean;
  repeatCount: number;
  active: boolean;
  repeated: boolean;
}

export interface ITaskElementsCollection {
  baseElements: IBase[];
  journals: IJournal[];
  relations: IRelation[];
  checklists: IChecklistItem[];
  timeLimits: ITimeLimit[];
}

export interface ITaskElement {
  baseElements: IBase;
  journals: IJournal | null;
  relations: IRelation | null;
  checklists: IChecklistItem | null;
  timeLimits: ITimeLimit | null;
}

export class TasksService {
  static getTasks = async() => {
    let config: AppConfig = require('./config.json');
    const bearerToken = localStorage.getItem('bearerToken');
    const rid = localStorage.getItem('rid');

    const response = await axios.get<ITaskElementsCollection>(config.apiURI + `/roots/${rid}/tasks`, {
      headers: { Authorization: `Bearer ${bearerToken}` },
    });

    return response.data;
  }

  static createTaskBase = async(baseElementDTO: IBaseDTO) => {
    let config: AppConfig = require('./config.json');
    const bearerToken = localStorage.getItem('bearerToken');
    const rid = localStorage.getItem('rid');

    let response = await axios.post(config.apiURI + `/roots/${rid}/tasks`, {
      name: baseElementDTO.name,
      description: baseElementDTO.description,
    }, {
      headers: { Authorization: `Bearer ${bearerToken}` },
    });

    return response.data;
  }

  static updateTasks = async(taskElements: ITaskElementsCollection) => {
    let config: AppConfig = require('./config.json');
    const rid = localStorage.getItem('rid');
    const bearerToken = localStorage.getItem('bearerToken');
    let response = await axios.patch(
      config.apiURI + `/roots/${rid}/tasks`,
      taskElements,
      {
        headers: { Authorization: `Bearer ${bearerToken}` },
      }
    );
    return response;
  }

  static deleteTask = async(bid: string) => {
    let config: AppConfig = require('./config.json');
    const rid = localStorage.getItem('rid');
    const bearerToken = localStorage.getItem('bearerToken');
    let response = await axios.delete(
      config.apiURI + `/roots/${rid}/tasks/${bid}`,
      {
        headers: { Authorization: `Bearer ${bearerToken}` },
      }
    );
    return response;
  }
}
