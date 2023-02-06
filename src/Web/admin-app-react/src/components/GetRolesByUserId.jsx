import { useEffect, useState } from "react";
import axios from "axios";
import Badge from 'react-bootstrap/Badge';

export default function GetRolesByUserId(props) {

    const { userId } = props;
    const [roles, setRoles] = useState([]);

    useEffect(() => {
        axios.get(`http://localhost:21001/roles?userId=${userId}`)
            .then((res) => {
                setRoles(res.data);
            })
            .catch((err) => console.log(err));
    }, [])

    return (
        <div>
            {(roles) && (roles).map((role, i) => {
                return (
                    <Badge
                        style={{marginRight: 10}}
                        key={i} pill bg="info"
                    >
                        {
                            role?.type == 0 ? "Student" 
                            : role?.type == 1 ? "Instructor" 
                            : role?.type == 2 ? "Publisher" 
                            : role?.type == 3 ? "Admin" 
                            : ""
                        }
                    </Badge>
                );
            })}
        </div>
    );
}