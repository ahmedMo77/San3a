import AuthLayout from "../../../components/auth/authLayout";
import SignInForm from "../../../components/auth/forms/SignIn/signInForm";
import ImagePlaceholder from "../../../sharedComponents/imagePlaceholder";
import AuthBox from "../../../components/auth/authBox";

const LoginPage = () => {
  return (
    <AuthLayout>
      <AuthBox>
        <SignInForm />
      </AuthBox>
      <ImagePlaceholder imageId="left" extension="png" height="520px" />
    </AuthLayout>
  );
};

export default LoginPage;
