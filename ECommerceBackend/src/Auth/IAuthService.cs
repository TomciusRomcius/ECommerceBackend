namespace ECommerce.Auth
{
	public interface IAuthService
	{
		public Task<AuthResponseDto> SignUpWithPassword(SignUpWithPasswordRequestDto signUpWithPasswordRequestDto);
		public Task<AuthResponseDto> SignInWithPassword(SignInWithPasswordRequestDto signInWithPasswordRequestDto);
	}
}