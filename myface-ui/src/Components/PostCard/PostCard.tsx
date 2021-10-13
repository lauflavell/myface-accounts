import React, { useContext} from "react";
import {createInteraction, Post} from "../../Api/apiClient";
import {Card} from "../Card/Card";
import "./PostCard.scss";
import {Link} from "react-router-dom";
import { LoginContext } from "../../Components/LoginManager/LoginManager";

interface PostCardProps {
    post: Post;
}

export function PostCard(props: PostCardProps): JSX.Element {


const loginContext = useContext(LoginContext);


    return (
        <Card>
            <div className="post-card">
                <img className="image" src={props.post.imageUrl} alt=""/>
                <div className="message">{props.post.message}</div>
                <button type="submit"  onClick={() => createInteraction(loginContext.logOut, loginContext.username, loginContext.password, {postId: props.post.id, interactionType: 0 })}> 👍</button> {props.post.likes.length}
                <button className="form-input"type="submit"  onClick={() => createInteraction(loginContext.logOut, loginContext.username, loginContext.password, {postId: props.post.id, interactionType: 1})}> 👎</button> {props.post.dislikes.length}
                <div className="user">
                    <img className="profile-image" src={props.post.postedBy.profileImageUrl} alt=""/>
                    <Link className="user-name" to={`/users/${props.post.postedBy.id}`}>{props.post.postedBy.displayName}</Link> 
                       
                </div>
            </div>
        </Card>
    );                                 
}