import React, { useContext, useState } from "react";
import { createInteraction, deletePost, Post } from "../../Api/apiClient";
import { Card } from "../Card/Card";
import "./PostCard.scss";
import { Link } from "react-router-dom";
import { LoginContext } from "../../Components/LoginManager/LoginManager";
import { BrowserRouter as Router, Switch, Route, Redirect } from "react-router-dom";

interface PostCardProps {
    post: Post;

}

export function PostCard(props: PostCardProps): JSX.Element {


    const loginContext = useContext(LoginContext);
    const [isDeleted, setIsDeleted] = useState(false);

    if (isDeleted) {
        return <Redirect to="/" />
    }


    return (
        <Card>
            <div className="post-card">
                <img className="image" src={props.post.imageUrl} alt="" />
                <div className="message">{props.post.message}</div>
                <button type="submit" onClick={() => createInteraction(loginContext.logOut, loginContext.username, loginContext.password, { postId: props.post.id, interactionType: 0 }).then(response => {loginContext.setLikes(response.likes)})}> 👍</button> {props.post.likes.length}
                <button className="active" type="submit" onClick={() => createInteraction(loginContext.logOut, loginContext.username, loginContext.password, { postId: props.post.id, interactionType: 1 }).then(response => {loginContext.setDislikes(response.dislikes)})}> 👎</button> {props.post.dislikes.length}
                {loginContext.role == 1 || loginContext.userId == props.post.postedBy.id ?
                    <button type="submit" onClick={() => deletePost(loginContext.logOut, loginContext.username, loginContext.password, props.post.id).then(() => setIsDeleted(true))}> Delete </button> : <> </>}
                <div className="user">
                    <img className="profile-image" src={props.post.postedBy.profileImageUrl} alt="" />
                    <Link className="user-name" to={`/users/${props.post.postedBy.id}`}>{props.post.postedBy.displayName}</Link>
                </div>
            </div>
        </Card>
    );
}
