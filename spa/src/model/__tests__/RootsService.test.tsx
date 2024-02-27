import axios from 'axios';
import { AppConfig } from '../config';
import { IRoot, RootsService } from '../RootsService';

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Root service', () => {
  let config: AppConfig = require('./../config.json');

  afterEach(() => {
    jest.clearAllMocks();
  });

  test('test get roots', async () => {
    // Arrange
    mockedAxios.get.mockResolvedValueOnce({
      data: [
      { rid: 'rid1', name: 'name1', password: 'password1' },
      { rid: 'rid2', name: 'name2', password: 'password2' },
      { rid: 'rid3', name: 'name3', password: 'password3' }
      ]
    });

    // Act
    let result = await RootsService.getRoots({limit: 10, offset: 0});

    // Assert
    expect(mockedAxios.get).toHaveBeenCalledWith(config.apiURI + `/roots`, {"params": {"limit": 10, "offset": 0}});
    expect(result).toEqual([
      { rid: 'rid1', name: 'name1', password: 'password1' },
      { rid: 'rid2', name: 'name2', password: 'password2' },
      { rid: 'rid3', name: 'name3', password: 'password3' }
      ]);
  });

  test('test create root', async () => {
    // Arrange
    let testRoot: IRoot = { rid: 'rid', name: 'name', password: 'password' };
    mockedAxios.post.mockResolvedValueOnce({ data: testRoot });
  
    // Act
    const result = await RootsService.createRoot(testRoot);
  
    // Assert
    expect(mockedAxios.post).toHaveBeenCalledWith(
      `${config.apiURI}/roots`,
      testRoot // The request payload should match the test root object
    );
    expect(result).toEqual(testRoot); // The method should return the root object from the response
  });

  test('test update root', async () => {
    // Arrange
    let testRoot: IRoot = { rid: 'rid', name: 'updatedName', password: 'updatedPassword' };
    const updatedRoot = { ...testRoot, name: 'updatedName', password: 'updatedPassword' };
    mockedAxios.patch.mockResolvedValueOnce({ data: updatedRoot });
    localStorage.setItem('bearerToken', 'test-bearer-token');
  
    // Act
    const result = await RootsService.updateRoot(testRoot);
  
    // Assert
    expect(mockedAxios.patch).toHaveBeenCalledWith(
      `${config.apiURI}/roots/${testRoot.rid}`,
      { name: testRoot.name, password: testRoot.password },
      { headers: { Authorization: `Bearer test-bearer-token` } }
    );
    expect(result).toEqual(updatedRoot);
  });

  test('test delete root', async () => {
    // Arrange
    localStorage.setItem('bearerToken', 'test-bearer-token');
  
    // Act
    await RootsService.deleteRoot('rid');
  
    // Assert
    expect(mockedAxios.delete).toHaveBeenCalledWith(
      `${config.apiURI}/roots/rid`,
      { headers: { Authorization: `Bearer test-bearer-token` } }
    );
  });

  test('test auth root', async () => {
    // Arrange
    let testRoot: IRoot = { rid: 'rid', name: 'name', password: 'password' };
    // Mock the axios response to return a token directly in `data`
    mockedAxios.get.mockResolvedValueOnce({ status: 200, data: 'token' });
  
    // Act
    const authSuccess = await RootsService.authRoot(testRoot);
  
    // Assert
    expect(authSuccess).toBeTruthy(); // Expect the authRoot to return true for successful authentication
    expect(localStorage.getItem('bearerToken')).toBe('token');
    expect(localStorage.getItem('rid')).toBe('rid');
    expect(localStorage.getItem('name')).toBe('name');
  
    // Check if axios.get was called with the correct URL and query parameters
    expect(mockedAxios.get).toHaveBeenCalledWith(
      `${config.apiURI}/roots/${testRoot.rid}/auth?password=${testRoot.password}`
    );
  });
});
