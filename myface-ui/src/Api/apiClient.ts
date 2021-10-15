export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    id: number;
    user: User;
    type: string;
    date: string;
    likes: number;
    dislikes: number;
}

export interface NewInteraction {
    postId: number;
    interactionType: number;
}    

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
}

export interface LoginDetails {
    username: string;
    password: string;
    logOut: () => void;
}
export interface LoginResponse {
    role: number;
    userId: number;
}

export async function fetchUsers(LogOut: () => void, username: string, password: string, searchTerm: string, page: number, pageSize: number): Promise<ListResponse<User>> {
    const response = await fetch(`https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }

    return await response.json();
}

export async function fetchUser(LogOut: () => void, username: string, password: string, userId: string | number): Promise<User> {
    const response = await fetch(`https://localhost:5001/users/${userId}`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }
    return await response.json();
}

export async function fetchPosts(LogOut: () => void, username: string, password: string, page: number, pageSize: number): Promise<ListResponse<Post>> {

    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }
    return await response.json();
}

export async function fetchPostsForUser(LogOut: () => void, username: string, password: string, page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }
    return await response.json();
}

export async function fetchPostsLikedBy(LogOut: () => void, username: string, password: string, page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }
    return await response.json();
}

export async function fetchPostsDislikedBy(LogOut: () => void, username: string, password: string, page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }
    return await response.json();
}

export async function createPost(LogOut: () => void, username: string, password: string, newPost: NewPost) {
    const response = await fetch(`https://localhost:5001/posts/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        },
        body: JSON.stringify(newPost),
    });
    if (response.status === 401) {
        LogOut();
    }

    if (!response.ok) {
        throw new Error(await response.json())
    }
}

export async function createInteraction(LogOut: () => void, username: string, password: string, newInteraction: NewInteraction): Promise<Interaction> {
    const response = await fetch(`https://localhost:5001/interactions/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        },
        body: JSON.stringify(newInteraction),
    });
    if (response.status === 401) {
        LogOut();
    }

    if (!response.ok) {
        throw new Error(await response.json())
    }
    return await response.json();
}

export async function deletePost(LogOut: () => void, username: string, password: string, postId: number) {
    const response = await fetch(`https://localhost:5001/posts/${postId}`, {
        method: "DELETE",
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        },
        
    });
    if (response.status === 401) {
        LogOut();
    }
    if (response.status === 403) {
        window.alert("Access Denied")
    }
}

export async function loginUser(LogOut: () => void, username: string, password: string): Promise<LoginResponse> {
    const response = await fetch(`https://localhost:5001/login`, {
        headers: {
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    });
    if (response.status === 401) {
        LogOut();
    }
    return await response.json();
    
}

