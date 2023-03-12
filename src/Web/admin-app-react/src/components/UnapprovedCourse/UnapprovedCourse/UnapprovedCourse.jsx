import { useParams } from "react-router";
import instance from "../../../api/request";
import LoadingSpinner from "../../LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";

export default function UnapprovedCourse() {
    const { id } = useParams();
    console.log(id);

    const [isLoading, setIsLoading] = useState(false);
    useEffect(() => {
       setIsLoading(true);
       instance
            .get(`lessons?courseId=${id}`)
            .then((res) => {
                console.log(res.data);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, []);

    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <p>course</p>
        </div>
    );
}
