import { useParams } from "react-router";
import instance from "../../../api/request";
import LoadingSpinner from "../../LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";

export default function Material() {
    const { materialId } = useParams();
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        instance.get(`units/${materialId}/material`)
        .then((res) => {
            console.log(res.data);
        })
        .finally(() => setIsLoading(false))
    }, []);

    if(isLoading) return <LoadingSpinner />

    return (
        <div>Material</div>
    )
}