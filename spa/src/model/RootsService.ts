import axios from 'axios';
import { AppConfig } from "./config"

export interface IRoot {
  rid: string;
  name: string;
  password: string;
}

export interface IFetchParams {
  limit?: number;
  offset?: number;
}

export interface AuthParams {
  bearerToken: string;
  rid: string;
  name: string;
}

export interface IRootCreate {
  name: string;
  password: string;
}

export class RootsService {
  static getRoots = async (params: IFetchParams) => {
    let config: AppConfig = require('./config.json');
    let roots: IRoot[] = [];
    try {
      const response = await axios.get<IRoot[]>(
        config.apiURI + '/roots',
        Object.keys(params).length ? { params } : undefined
      );
      roots = response.data;
    } catch (error) {
      console.error('Error refreshing roots:', error);
    }
    return roots;
  };

  static getRoot = async (rid: string) => {
    let config: AppConfig = require('./config.json');
    try {
      const bearerToken = localStorage.getItem('bearerToken');
      const response = await axios.get<IRoot>(`${config.apiURI}/roots/${rid}`, {
        headers: {
          Authorization: `Bearer ${bearerToken}`
        }
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching root with ID ${rid}:`, error);
      return null; // Or handle the error as you see fit
    }
  };

  static createRoot = async (root: IRootCreate) => {
    let config: AppConfig = require('./config.json');
    const response = await axios.post<IRoot>(config.apiURI + '/roots', root);
    return response.data;
  }

  static updateRoot = async (root: IRoot) => {
    let config: AppConfig = require('./config.json');
    const bearerToken = localStorage.getItem('bearerToken');
    
    const response = await axios.patch(
      `${config.apiURI}/roots/${root.rid}`,
      { name: root.name, password: root.password },
      { headers: { Authorization: `Bearer ${bearerToken}` } }
    );
  
    return response.data;
  };

  static deleteRoot = async (rid: string) => {
    let config: AppConfig = require('./config.json');

    const bearerToken = localStorage.getItem('bearerToken');
    await axios.delete(config.apiURI + `/roots/${rid}`, {
      headers: {
        Authorization: `Bearer ${bearerToken}`
      }
    });
  }

  static authRoot = async (root: IRoot) => {
    let config: AppConfig = require('./config.json');
  
    try {
      const response = await axios.get(`${config.apiURI}/roots/${root.rid}/auth?password=${root.password}`);
      if (response.status === 200) {
        localStorage.setItem('bearerToken', response.data); // Assuming the token is directly in `data`
        localStorage.setItem('rid', root.rid);
        localStorage.setItem('name', root.name);
        return true; // Indicate success
      }
      return false; // Indicate failure or unauthorized
    } catch (error) {
      console.error('Error in authRoot:', error);
      return false; // Indicate failure on error
    }
  }
}
