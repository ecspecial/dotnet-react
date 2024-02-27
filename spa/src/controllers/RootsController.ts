import { IFetchParams, IRoot, IRootCreate, RootsService, AuthParams } from '../model/RootsService';

export class RootsController {
  static fetchRoots = async (params: IFetchParams) => {
    let roots = await RootsService.getRoots({ limit: params.limit, offset: params.offset });
    return roots;
  }

  static getRoot = async (rid: string) => {
    return await RootsService.getRoot(rid);
  };

  static createRoot = async (root: IRootCreate) => {
    return await RootsService.createRoot(root);
  }

  static deleteRoot = async (rid: string) => {
    return await RootsService.deleteRoot(rid);
  }

  static updateRoot = async (root: IRoot) => {
    return await RootsService.updateRoot(root);
  }

  static authRoot = async (root: IRoot) => {
    return await RootsService.authRoot(root);
  }

  static setAuthRoot = async(params: AuthParams) => {
    localStorage.setItem('bearerToken', params.bearerToken);
    localStorage.setItem('rid', params.rid);
    localStorage.setItem('name', params.name); 
  }

  static unauthRoot = async() => {
    localStorage.setItem('bearerToken', '');
    localStorage.setItem('rid', '');
    localStorage.setItem('name', ''); 
  }
}
