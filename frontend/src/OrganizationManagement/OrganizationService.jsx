import { postCall } from "../API/postCall";
import { getCall } from "../API/getCall";

export const postOrganization = async (token, data, roleId) => {
    const stringData = JSON.stringify(data);
    
    try {
        const response = await postCall(stringData, "/api/organizations", "Error posting a new user", token);
        console.log(response);
        return response;
    } catch (error) {
        console.error(error);
        throw error;
    }

}

export const getUsers = (token) => {
    return getCall("/api/organizations?pageIndex=1&pageSize=9999999", token, "Error getting users")
        .then((response) => {
            console.log("Get organisations successful!");
            return response;
        });
}

export const buildOrgaNameArray = (array) => {
    let result = [];
    if(array !== null){
        array.forEach((a, index) => 
            result[index] = a.name)
        
            return result;
        }
    }


export const getOrgaIdByName = (array, name) => {
    let result = null;
    array.forEach((a) => 
     {if(a.name === name){
        console.log(a.id);
        result = a.id}}
)
    return result;
}

export const addParentNames = (arr) => {
    // Create a map from id to name for quick lookup
    const idToNameMap = new Map();
    arr.forEach(obj => {
      idToNameMap.set(obj.id, obj.name);
    });
  
    // Add parentName attribute based on parentId
    arr.forEach(obj => {
      if (obj.parentId !== null && idToNameMap.has(obj.parentId)) {
        obj.parentName = idToNameMap.get(obj.parentId);
      }
    });
  
    return arr;
  }