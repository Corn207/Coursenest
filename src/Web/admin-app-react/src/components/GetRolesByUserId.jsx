// import { useEffect, useState } from "react";
import Badge from 'react-bootstrap/Badge';
// import instance from "../api/request";

export default function GetRolesByUserId(props) {

    // const { userId } = props;
    // const [roles, setRoles] = useState([]);

    // useEffect(() => {
    //     instance.get(`roles/userId=${userId}`)
    //         .then((res) => {
    //             setRoles(res.data);
    //         })
    //         .catch((err) => console.log(err));
    // }, [])

    const { roles } = props;

    return (
        <div>
            {(roles) && (roles).map((role, i) => {
                return (
                    <Badge
                        style={{marginRight: 10}}
                        key={i} pill bg={(role == 0) ? "info" : (role == 1) ? "secondary" : (role == 2) ? "success" : (role == 3) ? "dark" : ""}
                    >
                        {
                            role == 0 ? "Student" 
                            : role == 1 ? "Instructor" 
                            : role == 2 ? "Publisher" 
                            : role == 3 ? "Admin" 
                            : ""
                        }
                    </Badge>
                );
            })}
        </div>
    );
}