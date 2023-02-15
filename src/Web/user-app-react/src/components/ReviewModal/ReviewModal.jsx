import Modal from 'react-bootstrap/Modal';
import { Button } from 'react-bootstrap';

export default function ReviewModal(props) {
    const { show, setShow } = props;
    const handleClose = () => setShow(false);

    return (
        <div>
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false} size="lg">
                <Modal.Header closeButton>
                    <Modal.Title>Review Form</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {/* <h4>{course.title}</h4> */}
                    <h4>Comment</h4>
                    <textarea placeholder="“Give any additional context on what happened.”" rows="3"></textarea>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary">Cancel</Button>
                    <Button variant="success">Save</Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}
