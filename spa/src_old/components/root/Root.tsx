import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Root.css';
import '../../App.css';

interface IRoot {
  rid?: string;
  name: string;
  password: string;
}

interface IRootResponse {
  rid: string;
  name: string;
  password: string;
}

interface Params {
  limit?: number;
  offset?: number;
}

const Root = () => {
  const [roots, setRoots] = useState<IRoot[]>([]);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [refreshModalOpen, setRefreshModalOpen] = useState(false);
  const [limit, setLimit] = useState<number>(10);
  const [offset, setOffset] = useState<number>(0);
  const [authorizationPopupOpen, setAuthorizationPopupOpen] = useState(false);
  const [isAuthorized, setIsAuthorized] = useState<boolean>(false);
  const [password, setPassword] = useState('');
  const [authorizationError, setAuthorizationError] = useState<string | null>(null);
  const [id, setId] = useState('');
  const [name, setName] = useState('');
  // New state for editing
  const [isEditing, setIsEditing] = useState(false);
  const [editName, setEditName] = useState('');
  const [editPassword, setEditPassword] = useState('');

  const fetchRoots = async () => {
    try {
      const response = await axios.get<IRootResponse[]>('http://localhost:7777/api/v1/roots');
      if (response && response.data) {
        setRoots(response.data);
      } else {
        console.error('Error fetching roots: Response data is undefined');
      }
    } catch (error) {
      console.error('Error fetching roots:', error);
    }
  };
  
  useEffect(() => {
    fetchRoots();
  }, []);

  // Function to toggle edit mode
  const handleEditToggle = (root: IRoot) => {
    setIsEditing(!isEditing);
    setEditName(root.name ?? ''); // Fallback to empty string if name is undefined
    setEditPassword(root.password ?? ''); // Fallback to empty string if password is undefined
    if (root.rid) { // Check if rid is defined
      setId(root.rid);
    }
  };

  // Function to handle fetching a single root
  const handleFetchSingleRoot = async () => {
    try {
      const bearerToken = localStorage.getItem('bearerToken');
      const response = await axios.get<IRootResponse>(`http://localhost:7777/api/v1/roots/${id}`, {
        headers: {
          Authorization: `Bearer ${bearerToken}`
        }
      });
      // Update state with the single root data
      setRoots([response.data]);
    } catch (error) {
      console.error('Error fetching single root:', error);
    }
  };

  const handleEditSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const bearerToken = localStorage.getItem('bearerToken');
    try {
      const response = await axios.patch(`http://localhost:7777/api/v1/roots/${id}`, {
        name: editName,
        password: editPassword,
      }, {
        headers: {
          Authorization: `Bearer ${bearerToken}`
        }
      });
      console.log(response.data);
      // Refresh roots or update UI accordingly
      window.alert('Account updated successfully!'); // Alert for successful update
    } catch (error) {
      console.error('Error updating root:', error);
    }
  };

  const handleDelete = async (rootId: string) => {
    const bearerToken = localStorage.getItem('bearerToken');
    try {
      const response = await axios.delete(`http://localhost:7777/api/v1/roots/${rootId}`, {
        headers: {
          Authorization: `Bearer ${bearerToken}`
        }
      });
      // Check if the deletion was successful based on the response status code
      if (response.status === 200) {
        // Fetch roots again after successful deletion
        localStorage.clear();
        setIsAuthorized(false);
        await fetchRoots();
        
        // Optionally, you can also display an alert for successful deletion
        window.alert('Account deleted successfully!');
      }
    } catch (error) {
      console.error('Error deleting root:', error);
    }
  };

  const handleCreateRoot = async (root: IRoot) => {
    try {
      const response = await axios.post<IRootResponse>('http://localhost:7777/api/v1/roots', root);
      setRoots([...roots, response.data]);
    } catch (error) {
      console.error('Error creating root:', error);
    }
  };

  const handleRefreshRoots = async () => {
    try {
      const params: Params = {};
      if (!isNaN(limit)) {
        params.limit = limit;
      }
      if (!isNaN(offset)) {
        params.offset = offset;
      }

      const response = await axios.get<IRootResponse[]>(
        'http://localhost:7777/api/v1/roots',
        Object.keys(params).length ? { params } : undefined
      );
      setRoots(response.data);
    } catch (error) {
      console.error('Error refreshing roots:', error);
    }
  };

  const handleCreateSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const newRoot: IRoot = {
      name: formData.get('name') as string,
      password: formData.get('password') as string,
    };
    await handleCreateRoot(newRoot);
    setCreateModalOpen(false);
  };

  const handleRefreshSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const form = event.currentTarget;
    const newLimit = parseInt(form.limit.value, 10) || 0;
    const newOffset = parseInt(form.offset.value, 10) || 0;

    setLimit(newLimit);
    setOffset(newOffset);

    await handleRefreshRoots();
    setRefreshModalOpen(false);
  };

  // Update handleAuthorization function to set isAuthorized state
const handleAuthorization = async () => {
  try {
    const response = await axios.get(`http://localhost:7777/api/v1/roots/${id}/auth?password=${password}`);
    localStorage.setItem('bearerToken', response.data);
    localStorage.setItem('rid', id);
    setAuthorizationPopupOpen(false);
    setAuthorizationError(null);
    const authenticatedRoot = roots.filter((root) => root.rid === id);
    setName(authenticatedRoot[0].name)
    localStorage.setItem('name', name);
    setRoots(authenticatedRoot);
    setIsAuthorized(true); // Set authorization status to true
  } catch (error: any) {
    if (error.response && error.response.data && error.response.data.message) {
      const errorMessage = error.response.data.message;
      const statusCode = error.response.status;
      setAuthorizationError(`Authorization failed with status ${statusCode}: ${errorMessage}`);
    } else if (error.response && error.response.status) {
      const statusCode = error.response.status;
      setAuthorizationError(`Authorization failed with status ${statusCode}`);
    } else {
      setAuthorizationError('Authorization failed. Please try again.');
    }
  }
};

  return (
    <div className='appContainer'>
      <h2 className='title'>Root App</h2>
      <div className='buttonContainer'>
        <button className='button' onClick={() => setCreateModalOpen(true)}>Create Root</button>
        <button className='button' onClick={() => setRefreshModalOpen(true)}>Refresh Roots</button>
        <button className='button' onClick={() => setAuthorizationPopupOpen(true)}>Authorize</button>
        {(isAuthorized || localStorage.getItem('bearerToken')) && (
          <button className='button' onClick={handleFetchSingleRoot}>Find Single Root</button>
        )}
      </div>
      {createModalOpen && (
        <form className='form' onSubmit={handleCreateSubmit}>
          <label className='label'>
            Name:
            <input className='input' name="name" type="text" required />
          </label>
          <label className='label'>
            Password:
            <input className='input' name="password" type="password" required />
          </label>
          <div className='buttonContainer'>
            <button className='button' type="submit">Submit</button>
            <button className='button cancelButton' onClick={() => setCreateModalOpen(false)}>Cancel</button>
          </div>
        </form>
      )}
      {refreshModalOpen && (
        <form className='form' onSubmit={handleRefreshSubmit}>
          <label className='label'>
            Limit:
            <input
              className='input'
              name="limit"
              type="number"
              value={limit ?? ''}
              onChange={(e) => setLimit(parseInt(e.target.value, 10))}
            />
          </label>
          <label className='label'>
            Offset:
            <input
              className='input'
              name="offset"
              type="number"
              value={offset ?? ''}
              onChange={(e) => setOffset(parseInt(e.target.value, 10))}
            />
          </label>
          <div className='buttonContainer'>
            <button className='button' type="submit">Refresh</button>
            <button className='button cancelButton' onClick={() => setRefreshModalOpen(false)}>Cancel</button>
          </div>
        </form>
      )}
      {authorizationPopupOpen && (
        <div className='popup'>
          <form className='form' onSubmit={(e) => { e.preventDefault(); handleAuthorization(); }}>
            <label className='label'>
              ID:
              <input className='input' type='text' value={id} onChange={(e) => setId(e.target.value)} />
            </label>
            <label className='label'>
              Password:
              <input className='input' type='password' value={password} onChange={(e) => setPassword(e.target.value)} />
            </label>
            {authorizationError && <p className='error'>{authorizationError}</p>}
            <div className='buttonContainer'>
              <button className='button' type='submit'>Authorize</button>
              <button className='button cancelButton' onClick={() => setAuthorizationPopupOpen(false)}>Cancel</button>
            </div>
          </form>
        </div>
      )}
      <div className='rootList'>
        {roots.map((root) => (
          <div className='rootItem' key={root.rid}>
            <p>{`rid: [ ${root.rid} ]`}</p>
            <p>{`name: [ ${root.name} ]`}</p>
            {localStorage.getItem('rid') === root.rid && (
              <>
                <button className='editButton' onClick={() => handleEditToggle(root)}>Edit</button>
                <button className='deleteButton' onClick={() => root.rid && handleDelete(root.rid)}>Delete Account</button>
                {isEditing && (
                <form onSubmit={handleEditSubmit}>
                  <input className='input' value={editName} onChange={(e) => setEditName(e.target.value)} />
                  <input className='input' type="password" value={editPassword} onChange={(e) => setEditPassword(e.target.value)} />
                  <button className='updateButton' type="submit">Update</button>
                </form>
              )}
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default Root;
